#load "../models/models.csx"

using System.Collections;

public abstract class MoveStrategy : IEnumerator<CellPosition>
{
    public MoveStrategy(CellPosition pos, bool isBlocking = true)
    {
        _initial = new CellPosition(pos);
        _current = new CellPosition(pos);
        IsBlocking = isBlocking;
    }

    public bool IsBlocking {get; protected set; }

    public CellPosition Current => _current;
    public abstract bool MoveNext();

    public virtual void Reset()
    {
        _current = new CellPosition(_initial.Row, _initial.Col);
    }

    public void Dispose() {}

    protected CellPosition _initial;
    protected CellPosition _current;

    object IEnumerator.Current => Current;
}

public class MoveUp : MoveStrategy
{
    public MoveUp(CellPosition pos) : base(pos) {}
    
    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row+1, _current.Col);
        return _current.Row < 9;
    }
}

public class MoveDown : MoveStrategy
{
    public MoveDown(CellPosition pos) : base(pos) {}
    
    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row-1, _current.Col);
        return _current.Row > 0;
    }

}

public class MoveLeft : MoveStrategy
{
    public MoveLeft(CellPosition pos) : base(pos) {}
    
    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row, _current.Col-1);
        return _current.Col > 0;
    }
}

public class MoveRight : MoveStrategy
{
    public MoveRight(CellPosition pos) : base(pos) {}
    
    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row, _current.Col+1);
        return _current.Col < 9;
    }
}

public class MoveUpLeft : MoveStrategy
{
    public MoveUpLeft(CellPosition pos) : base(pos) {}

    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row+1, _current.Col-1);
        return _current.Col > 0 && _current.Row < 9;        
    }
}

public class MoveUpRight : MoveStrategy
{
    public MoveUpRight(CellPosition pos) : base(pos) {}

    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row+1, _current.Col+1);
        return _current.Col < 9 && _current.Row < 9;        
    }   
}

public class MoveDownLeft : MoveStrategy
{
    public MoveDownLeft(CellPosition pos) : base(pos) {}

    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row-1, _current.Col-1);
        return _current.Col > 0 && _current.Row > 0;        
    }  
}

public class MoveDownRight : MoveStrategy
{
    public MoveDownRight(CellPosition pos) : base(pos) {}

    public override bool MoveNext()
    {
        _current = new CellPosition(_current.Row-1, _current.Col+1);
        return _current.Col < 9 && _current.Row > 0;        
    }
}

public class MoveCollectionStrategy : MoveStrategy
{
    public MoveCollectionStrategy(CellPosition pos, List<CellPosition> deltas, bool isBlocking = false) : base(pos, isBlocking)
    {
        this.deltas = deltas;
    }

    public override bool MoveNext()
    {
        if(_ix >= deltas.Count)
            return false;

        _current = new CellPosition(_initial.Row + deltas[_ix].Row,
                                    _initial.Col + deltas[_ix].Col);
        _ix++;
        return true;
    }

    public override void Reset()
    {
        _ix = 0;
        base.Reset();
    }

    private List<CellPosition> deltas;
    private int _ix = 0;
}

public class MovePawnDown : MoveCollectionStrategy
{
    public MovePawnDown(CellPosition pos) : base(pos, GetDeltas()) {}

    private static List<CellPosition> GetDeltas()
    {
        List<CellPosition> list = new();
        list.Add(new CellPosition(-1, -1));
        list.Add(new CellPosition(-1, 0));
        list.Add(new CellPosition(-1, 1));
        return list;
    }
}

public class MovePawnUp : MoveCollectionStrategy
{
    public MovePawnUp(CellPosition pos) : base(pos, GetDeltas()) {}

    private static List<CellPosition> GetDeltas()
    {
        List<CellPosition> list = new();
        list.Add(new CellPosition(1, -1));
        list.Add(new CellPosition(1, 0));
        list.Add(new CellPosition(1, 1));
        return list;
    }
}

public class MoveKnight : MoveCollectionStrategy
{
    public MoveKnight(CellPosition pos) : base(pos, GetDeltas()) {}

    private static List<CellPosition> GetDeltas()
    {
        List<CellPosition> list = new();
        list.Add(new CellPosition(2,-1));
        list.Add(new CellPosition(2, 1));
        list.Add(new CellPosition(1,-2));
        list.Add(new CellPosition(1, 2));
        list.Add(new CellPosition(-1,-2));
        list.Add(new CellPosition(-1,2));
        list.Add(new CellPosition(-2,-1));
        list.Add(new CellPosition(-2,1));
        return list;
    }
}

public class MoveKing : MoveCollectionStrategy
{
    public MoveKing(CellPosition pos) : base(pos, GetDeltas()) {}

    private static List<CellPosition> GetDeltas()
    {
        List<CellPosition> list = new();
        list.Add(new CellPosition(1,-1));
        list.Add(new CellPosition(1, 0));
        list.Add(new CellPosition(1, 1));
        list.Add(new CellPosition(0,-1));
        list.Add(new CellPosition(0, 1));
        list.Add(new CellPosition(-1,-1));
        list.Add(new CellPosition(-1, 0));
        list.Add(new CellPosition(-1, 1));
        return list;
    }
}


public static class MoveStrategies
{
    public static List<MoveStrategy> Straight(CellPosition pos)
    {
        List<MoveStrategy> moves = new();
        moves.Add(new MoveUp(pos));
        moves.Add(new MoveLeft(pos));
        moves.Add(new MoveDown(pos));
        moves.Add(new MoveRight(pos));
        return moves;
    }

    public static List<MoveStrategy> Diagonal(CellPosition pos)
    {
        List<MoveStrategy> moves = new();
        moves.Add(new MoveUpLeft(pos));
        moves.Add(new MoveUpRight(pos));
        moves.Add(new MoveDownLeft(pos));
        moves.Add(new MoveDownRight(pos));
        return moves;
    }

    public static List<MoveStrategy> Queen(CellPosition pos)
    {
        List<MoveStrategy> moves = new();
        moves.AddRange(MoveStrategies.Straight(pos));
        moves.AddRange(MoveStrategies.Diagonal(pos));
        return moves;
    }
}
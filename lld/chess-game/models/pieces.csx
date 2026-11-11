#load "enums.csx"
#load "models.csx"
#load "entities.csx"
#load "../strategies/movement.csx"

public abstract class Piece
{
    public Piece(CellPosition position, Color color)
    {
        Position = position;
        Color = color;    
    }

    public CellPosition Position {get; set;}
    public Color Color {get; private set;}
    public abstract string GetSymbol();

    public virtual List<CellPosition> CandidateMoves(ChessBoard board)
    {
        List<CellPosition> candidateMoves = new();
        foreach(MoveStrategy moveStrategy in GetMoveStrategies())
        {
            while(moveStrategy.MoveNext())
            {
                CellPosition newPos = moveStrategy.Current;
                MoveResult result = board.CanMoveCore(this, newPos);
                
                if(result == MoveResult.Succeeded)
                    candidateMoves.Add(newPos);
                else if(moveStrategy.IsBlocking)
                    break;
            }
        }
        return candidateMoves;
    }

    public virtual bool CanReach(ChessBoard board, CellPosition newPos, out MoveResult moveResult)
    {
        foreach(MoveStrategy moveStrategy in GetMoveStrategies())
        {
            while(moveStrategy.MoveNext())
            {
                CellPosition pos = moveStrategy.Current;
                Piece p = board.GetAt(pos);
                
                if(pos == newPos)
                {
                    if(p is null || p.Color != this.Color)
                    {
                        moveResult = MoveResult.Succeeded;
                        return true;
                    }

                    moveResult = MoveResult.TargetOccupied;
                    return false;
                }

                if(p is not null && moveStrategy.IsBlocking)
                    break;
            }
        }

        moveResult = MoveResult.Unreachable;
        return false;
    }

    protected abstract List<MoveStrategy> GetMoveStrategies(); 
}

public class Rook : Piece
{
    public Rook(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♖" : "♜";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        return MoveStrategies.Straight(Position);
    }
}

public class Knight : Piece
{
    public Knight(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♘" : "♞";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        List<MoveStrategy> list = new();
        list.Add(new MoveKnight(Position));
        return list;
    }
}

public class Bishop : Piece
{
    public Bishop(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♗" : "♝";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        return MoveStrategies.Diagonal(Position);
    }

}

public class King : Piece
{
    public King(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♔" : "♚";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        List<MoveStrategy> list = new();
        list.Add(new MoveKing(Position));
        return list;
    }
}

public class Queen : Piece
{
    public Queen(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♕" : "♛";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        return MoveStrategies.Queen(Position);
    }
}

public class Pawn : Piece
{
    public Pawn(CellPosition position, Color color) : base(position, color) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "♙" : "♟";
    }

    protected override List<MoveStrategy> GetMoveStrategies()
    {
        List<MoveStrategy> list = new();        
        list.Add(Color == Color.White 
                    ? new MovePawnUp(Position)
                    : new MovePawnDown(Position));
        return list;
    }
}


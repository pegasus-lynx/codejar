#load "l1_position.csx"
#load "l3_board.csx"


public abstract class MoveStrategy
{
    public abstract bool CanReach(Position from, Position to, ChessBoard board);
}

public class RookStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        return from.Row == to.Row || from.Col == to.Col;
    }
}

public class BishopStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        Position diff = to - from;
        return Math.Abs(diff.Row) == Math.Abs(diff.Col);
    }
}

public class QueenStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        MoveStrategy rook = new RookStrategy();
        MoveStrategy bishop = new BishopStrategy();
        return rook.CanReach(from, to, board) || bishop.CanReach(from, to, board);
    }
}

public class KnightStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        int dr = Math.Abs(to.Row - from.Row);
        int dc = Math.Abs(to.Col - from.Col);

        return (dr == 1 && dc == 2) ||
               (dr == 2 && dc == 1);
    }    
}

public class PawnStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        Piece fp = board.GetPiece(from);
        Position diff = to - from;

        int dir = fp.Color == Color.White ? 1 : -1;
        int startRow = fp.Color == Color.White ? 1 : 6;

        if(diff.Row * dir <= 0)
            return false;  

        if(Math.Abs(diff.Col) > 1)
            return false;

        if(Math.Abs(diff.Col) == 1)
        {
            if(Math.Abs(diff.Row) != 1)
                return false;

            if(board.GetPiece(to) is Piece tp && tp.Color != fp.Color)
                return true;
        }

        if(Math.Abs(diff.Col) == 0)
        {
            if(Math.Abs(diff.Row) == 1 && board.GetPiece(to) is null)
                return true;

            if(Math.Abs(diff.Row) == 2 && board.GetPiece(to) is null
                && from.Row == startRow && board.GetPiece(to.Row-dir, to.Col) is null)
                return true;

            return false;
        }

        return false;
    }
}

public class KingStrategy : MoveStrategy
{
    public override bool CanReach(Position from, Position to, ChessBoard board)
    {
        Position diff = to-from;
        return Math.Abs(diff.Row) <= 1 && Math.Abs(diff.Col) <= 1;
    }
}
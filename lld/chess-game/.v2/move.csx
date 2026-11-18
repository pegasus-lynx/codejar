#load "cell.csx"
#load "board.csx"

// Movement strategies
//  They answer the question, if a piece can reach this cell (not considering what is on the board)

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

        if(diff.Row * dir <= 0)
            return false;  

        if(Math.Abs(diff.Row) == 2 && diff.Col == 0)
        {
            if(board.GetPiece(to) is null)
            {
                if(dir == 1 && from.Row == 2)
                    return true;

                if(dir == -1 && from.Row == 7)
                    return true;
            }

            return false;
        }

        if(Math.Abs(diff.Row) > 1 || Math.Abs(diff.Col) > 1)
            return false;

        return diff.Col == 0 || board.GetPiece(to) is not null;
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
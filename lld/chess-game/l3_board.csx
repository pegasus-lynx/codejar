#load "l1_enums.csx"
#load "l1_position.csx"
#load "l2_pieces.csx"
#load "l3_rules.csx"


public class Move
{
    public Move(Position from, Position to, Piece moved, Piece target)
    {
        From = from; 
        To = to;
        MovedPiece = moved;
        TargetPiece = target;
    }

    public Position From { get; init; }
    public Position To { get; init; }
    public Piece MovedPiece { get; init; }
    public Piece TargetPiece { get; init; }
}


public interface IBoardView
{
    public PieceInfo GetAt(int row, int col);
}


public class ChessBoard : IBoardView
{
    public ChessBoard()
    {
        InitializeBoard();
        captured_pieces = new();
    }

    public Piece GetPiece(Position pos) => GetPiece(pos.Row, pos.Col);

    public Piece GetPiece(int row, int col)
    {
        return Position.CheckBounds(row, col) ? pieces[row, col] : null;
    }

    public PieceInfo GetAt(int row, int col)
    {
        if(Position.CheckBounds(row, col))
            if(pieces[row, col] is Piece p)
                return new PieceInfo(p.Symbol, p.Color);

        return null;
    }

    public IEnumerable<Piece> GetPieces()
    {
        for(int r=0; r<8; r++)
            for(int c=0; c<8; c++)
                if(pieces[r,c] is not null)
                    yield return pieces[r,c];
    }

    public IEnumerable<Piece> GetPieces(Color color)
    {
        return GetPieces().Where(p => p.Color == color);
    }

    internal Move Move(Position from, Position to)
    {
        Piece fromPiece = GetPiece(from);
        Piece toPiece = GetPiece(to);

        if(toPiece is not null)
            captured_pieces.Add(toPiece);

        pieces[to.Row, to.Col] = fromPiece;
        fromPiece.Position = to;
        pieces[from.Row, from.Col] = null;

        return new Move(from, to, fromPiece, toPiece);
    }

    internal void UndoMove(Move move)
    {
        Position from = move.From;
        Position to = move.To;

        Piece movedPiece = move.MovedPiece;
        Piece targetPiece = move.TargetPiece;
    
        if(targetPiece is not null)
            captured_pieces.Remove(targetPiece);

        pieces[from.Row, from.Col] = movedPiece;
        movedPiece.Position = from;
        pieces[to.Row, to.Col] = targetPiece;
    }

    private void InitializeBoard()
    {
        for(int r=0; r<8; r++)
        {
            for(int c=0; c<8; c++)
            {
                Position pos = new(r, c);
                pieces[r,c] = CreatePieceForPosition(pos);
            }
        }

        Piece CreatePieceForPosition(Position position)
        {
            if(position.Row > 1 && position.Row < 6)
                return null;

            Color color = position.Row < 2 ? Color.White : Color.Black;

            if(position.Row == 6 || position.Row == 1)
                return new Pawn(position, color);

            int column = position.Col;
            if(column == 0 || column == 7)
                return new Rook(position, color);
            
            if(column == 1 || column == 6)
                return new Knight(position, color);

            if(column == 2 || column == 5)
                return new Bishop(position, color);

            return column == 3 ? new Queen(position, color)
                               : new King(position, color);
        }
    }

    private Piece[,] pieces = new Piece[8,8];
    private List<Piece> captured_pieces;
}
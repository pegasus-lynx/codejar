#load "enums.csx"
#load "cell.csx"
#load "pieces.csx"
#load "rules.csx"

public record class Move
{
    // public MoveKind Kind;
    public Position From;
    public Position To;
    public Piece MovedPiece;
    public Piece TargetPiece;
}


public interface IBoardObserver
{
    public void MoveMade(Move move);
}


public class ChessBoard
{
    public ChessBoard()
    {
        InitializeBoard();
        UpdateStateFlags();
        rulesEngine = new RulesEngine(this);
        observers = new();
        captured_pieces = new();
    }

    public BoardState State { get; private set; }

    public Color? StateColor { get; private set; }

    public MoveError Move(Position from, Position to, out Move move)
    {
        move = null;
        MoveError result = rulesEngine.ValidateMove(from, to);
        if(result != MoveError.None)
            return result;

        move = ExecuteMove(from, to);
        UpdateBoardState();

        foreach(IBoardObserver observer in observers)
            observer.MoveMade(move);

        return result;
    }

    public void Subscribe(IBoardObserver observer) => observers.Add(observer);

    public void Unsubscribe(IBoardObserver observer) => observers.Remove(observer);

    public Piece GetPiece(Position pos) => cells[pos.Row, pos.Col].Occupant;

    public Piece GetPiece(int row, int col)
    {
        return Position.CheckBounds(row, col) ? cells[row, col].Occupant : null;
    }

    public IEnumerable<Piece> GetPieces()
    {
        for(int r=1; r<=8; r++)
        {
            for(int c=1; c<=8; c++)
            {
                if(cells[r,c].IsOccupied())
                {
                    yield return cells[r,c].Occupant;
                }
            }
        }
    }

    private void UpdateBoardState()
    {
        UpdateStateFlags(BoardState.None, null);

        foreach(Color color in Enum.GetValues<Color>())
        {            
            if(rulesEngine.IsInCheckMate(color))
            {
                UpdateStateFlags(BoardState.CheckMate, color);
            }
            else if(rulesEngine.IsInCheck(color))
            {
                UpdateStateFlags(BoardState.Check, color);
            }
            else if(rulesEngine.IsInStaleMate(color))
            {
                UpdateStateFlags(BoardState.StaleMate, color);
            }

            if(State != BoardState.None)
                break;
        }
    }

    private Move ExecuteMove(Position from, Position to)
    {
        Piece fromPiece = GetPiece(from);
        Piece toPiece = GetPiece(to);

        if(toPiece is not null)
            captured_pieces.Add(toPiece);

        cells[to.Row, to.Col].Occupant = fromPiece;
        fromPiece.Position = to;
        cells[from.Row, from.Col].Occupant = null;

        Move move = new() { From = from, To = to, MovedPiece = fromPiece, TargetPiece = toPiece };
        return move;
    }


    private void InitializeBoard()
    {
        cells = new Cell[9,9];
        for(int r=1; r<=8; r++)
        {
            for(int c=1; c<=8; c++)
            {
                Position pos = new(r, c);
                Piece piece = CreatePieceForPosition(pos);
                cells[r,c] = new Cell(pos, piece);
            }
        }

        Piece CreatePieceForPosition(Position position)
        {
            if(position.Row > 2 && position.Row < 7)
                return null;

            Color color = position.Row < 3 ? Color.White : Color.Black;

            if(position.Row == 7 || position.Row == 2)
                return new Pawn(position, color);

            int column = position.Col;
            if(column == 1 || column == 8)
                return new Rook(position, color);
            
            if(column == 2 || column == 7)
                return new Knight(position, color);

            if(column == 3 || column == 6)
                return new Bishop(position, color);

            return column == 4 ? new Queen(position, color)
                               : new King(position, color);
        }
    }

    private void UpdateStateFlags(BoardState state = BoardState.None, Color? color = null)
    {
        State = state;
        StateColor = color;        
    }

    private Cell[,] cells;
    private List<Piece> captured_pieces;
    private RulesEngine rulesEngine;
    private List<IBoardObserver> observers;
}
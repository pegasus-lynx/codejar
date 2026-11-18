#load "models.csx"
#load "board.csx"
#load "enums.csx"
#load "pieces.csx"
#load "prints.csx"

public class ChessGame
{
    public ChessGame(User ublack, User uwhite)
    {
        PlayerBlack = new Player(ublack, Color.Black);       
        PlayerWhite = new Player(uwhite, Color.White);
        board = new ChessBoard();
        printer = new GamePrinter(this, board);
    }

    public Player PlayerBlack { get; private set; }
    public Player PlayerWhite { get; private set; }
    public GameState State { get; private set; } = GameState.InProgress;

    public Player Winner { get; private set; }

    public MoveError TakeTurn(string from, string to)
    {
        Position fromPos = Position.From(from);
        Position toPos = Position.From(to);
        return TakeTurn(fromPos, toPos);
    }

    public MoveError TakeTurn(Position from, Position to)
    {
        if(State == GameState.Finished)
            return MoveError.GameOver;

        Piece piece = board.GetPiece(from);
        if(piece is null)
            return MoveError.NoPiece;

        if(piece.Color != nextPlayerColor)
            return MoveError.WrongColor;

        MoveError result = board.Move(from, to, out Move move);
        if(result != MoveError.None)
            return result;

        if(board.State == BoardState.CheckMate || board.State == BoardState.StaleMate)
        {
            State = GameState.Finished;
            Winner = board.StateColor == Color.Black
                        ? PlayerWhite : PlayerBlack;
        }

        nextPlayerColor = nextPlayerColor == Color.White
            ? Color.Black
            : Color.White;

        return result;
    }

    public void Print() => printer.Print();

    private GamePrinter printer;
    private Color nextPlayerColor = Color.White;
    private ChessBoard board;
}
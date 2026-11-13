
public class ChessGame
{
    public ChessGame(User ublack, User uwhite)
    {
        PlayerBlack = new Player(ublack, Color.Black);       
        PlayerWhite = new Player(uwhite, Color.White);
        board = new ChessBoard();
    }

    public Player PlayerBlack { get; private set; }
    public Player PlayerWhite { get; private set; }
    public GameState State { get; private set; } = GameState.InProgress;

    public Player Winner { get; private set; }

    public bool TakeTurn(string from, string to)
    {
        CellPosition fromPos = CellPosition.From(from);
        CellPosition toPos = CellPosition.From(to);
        return TakeTurn(fromPos, toPos);
    }

    public bool TakeTurn(CellPosition from, CellPosition to)
    {
        if(State == GameState.Finished)
            return false;

        Piece piece = board.GetAt(from);
        if(piece is null || piece.Color != nextPlayerColor)
            return false;

        MoveResult result = board.CanMove(from, to);
        if(result != MoveResult.Succeeded)
            return false;

        board.Move(from, to);
        if(board.State == BoardState.CheckMateBlack || board.State == BoardState.CheckMateWhite)
        {
            State = GameState.Finished;
            Winner = board.State == BoardState.CheckMateBlack
                        ? PlayerWhite : PlayerBlack;
        }

        nextPlayerColor = nextPlayerColor == Color.White
            ? Color.Black
            : Color.White;

        return true;
    }

    public List<CellPosition> AvailableMoves(CellPosition from)
    {
        Piece piece = board.GetAt(from);
        if(piece is null || piece.Color != nextPlayerColor)
            return null;

        return piece.CandidateMoves(board);
    }

    public void Print()
    {
        Console.WriteLine($"{PlayerWhite.User.Name} (White) vs {PlayerBlack.User.Name} (Black)");
        Console.WriteLine($"Game: {State} | Board: {board.State}");

        if(State == GameState.Finished)
            Console.WriteLine($"Winner: {Winner.User.Name} ({Winner.Color})");
        else
        {
            Player nextPlayer = nextPlayerColor == Color.White ? PlayerWhite : PlayerBlack;
            Console.WriteLine($"Next: {nextPlayer.User.Name} ({nextPlayer.Color})");
        }

        board.Print();
    }

    private Color nextPlayerColor = Color.White;
    private ChessBoard board;
}
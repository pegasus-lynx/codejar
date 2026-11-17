#load "l1_enums.csx"
#load "l1_position.csx"
#load "l1_models.csx"
#load "l2_pieces.csx"
#load "l3_board.csx"
#load "l3_rules.csx"

public class ChessGame
{
    public ChessGame(User white, User black)
    {
        PlayerWhite = new Player(white, Color.White);
        PlayerBlack = new Player(black, Color.Black);       

        board = new ChessBoard();
        rulesEngine = new RulesEngine(board);
    }

    public Player PlayerBlack { get; private set; }
    public Player PlayerWhite { get; private set; }
    public Player Winner { get; private set; }

    public GameState State { get; private set; } = GameState.Ongoing;
    public Color CurrentMove { get; private set; } = Color.White;
    public bool IsGameOver => State == GameState.CheckMate || State == GameState.StaleMate;

    public IBoardView GetBoardView() => board as IBoardView;

    public MoveError TakeTurn(string from, string to)
    {
        Position fp = Position.From(from);
        Position tp = Position.From(to);
        return TakeTurn(fp, tp);
    }

    public MoveError TakeTurn(Position from, Position to)
    {

        if(IsGameOver)
            return MoveError.GameOver;

        if(!Position.CheckBounds(from.Row, from.Col))
            return MoveError.InvalidPosition;

        if(!Position.CheckBounds(to.Row, to.Col))
            return MoveError.InvalidPosition;

        if(board.GetPiece(from) is not Piece fromPiece)
            return MoveError.NoPiece;

        if(CurrentMove != fromPiece.Color)
            return MoveError.WrongColor;

        MoveError result = rulesEngine.Validate(fromPiece, to);
        if(result != MoveError.None)
            return result;

        board.Move(from, to);
        State = rulesEngine.EvaluateGameState(CurrentMove.Opposite());

        if(State == GameState.CheckMate)
            Winner = CurrentMove == Color.White ? PlayerWhite : PlayerBlack;    

        CurrentMove = CurrentMove.Opposite();
        return result;
    }

    private ChessBoard board;
    private RulesEngine rulesEngine;
}
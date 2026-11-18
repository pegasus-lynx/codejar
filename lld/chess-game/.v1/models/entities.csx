
#load "enums.csx"
#load "models.csx"
#load "pieces.csx"

public class ChessBoard
{
    public ChessBoard()
    {
        InitializeBoard();   
        State = BoardState.Normal;     
    }

    public BoardState State { get; private set; }

    public MoveResult Move(CellPosition from, CellPosition to)
    {
        MoveResult result = CanMove(from, to);
        if(result != MoveResult.Succeeded)
            return result;

        Piece toPiece = GetAt(to);
        if(toPiece is not null)
        {
            dead_pieces.Add(toPiece);
            active_pieces.Remove(to);
        }

        Piece fromPiece = GetAt(from);
        fromPiece.Position = to;
        active_pieces[to] = fromPiece;
        active_pieces.Remove(from);

        UpdateBoardState();
        return result;
    }
    
    public MoveResult CanMove(CellPosition from, CellPosition to)
    {
        Piece fromPiece = GetAt(from);
        if(fromPiece is null)
            return MoveResult.EmptyStart;

        MoveResult result;
        if(!fromPiece.CanReach(this, to, out result))
            return result;

        return CanMoveCore(fromPiece, to);
    }

    // Used by Piece only. Checks viability of moving.
    // The caller ensure 'to' position is in path of the piece. 
    // So, we skip the check.
    internal MoveResult CanMoveCore(Piece p, CellPosition to)
    {
        Piece toPiece = GetAt(to);
        if(toPiece is not null && toPiece.Color == p.Color)
            return MoveResult.TargetOccupied;

        MoveResult result = MoveResult.Succeeded;

        CellPosition from = p.Position;
        p.Position = to;
        active_pieces[to] = p; 
        active_pieces.Remove(from);

        if(IsChecked(p.Color))
            result = MoveResult.ExposesKing;

        p.Position = from;
        active_pieces[to] = toPiece;
        active_pieces[from] = p;

        return result;    
    }

    public bool IsChecked(Color color)
    {
        Piece kingPiece = active_pieces.Values.FirstOrDefault(x => x is King && x.Color == color);

        foreach(KeyValuePair<CellPosition, Piece> entry in active_pieces)
        {
            if(entry.Value is null)
                continue;

            Piece piece = entry.Value as Piece;
            if(piece.Color == color)
                continue;

            if(piece.CanReach(this, kingPiece.Position, out MoveResult result))
                return true;
        }

        return false;
    }

    public Piece GetAt(int r, int c) => GetAt(new CellPosition(r,c));
    public Piece GetAt(CellPosition pos)
    {
        if(active_pieces.ContainsKey(pos))
            return active_pieces[pos];
        return null;
    }

    public bool IsCellEmpty(CellPosition pos) => GetAt(pos) is null;

    public void Print()
    {
        StringBuilder board = new();
        board.AppendLine("    A B C D E F G H");

        for(int row = 8; row >= 1; row--)
        {
            board.Append($"{row,2}  ");

            for(int col = 1; col <= 8; col++)
            {
                Piece piece = GetAt(row, col);
                board.Append($"{piece?.GetSymbol() ?? " "} ");
            }

            board.AppendLine($" {row}");
        }

        board.AppendLine("    A B C D E F G H");
        Console.Write(board);
    }

    private void InitializeBoard()
    {
        void InitializePawns(Color color, int row)
        {
            for(int c=1; c<9; c++)
            {
                CellPosition pos = new CellPosition(row, c);
                active_pieces[pos] = new Pawn(pos, color);
            }
        };

        void InitializeOffense(Color color, int row)
        {
            for(int c=1; c<9; c++)
            {
                CellPosition pos = new CellPosition(row, c);

                if(c==1 || c==8)
                    active_pieces[pos] = new Rook(pos, color);
                else if(c==2 || c==7)
                    active_pieces[pos] = new Knight(pos, color);
                else if(c==3 || c==6)
                    active_pieces[pos] = new Bishop(pos, color);
                else if(c==4)
                    active_pieces[pos] = new Queen(pos, color);
                else if(c==5)
                    active_pieces[pos] = new King(pos, color);
            }
        }

        InitializeOffense(Color.White, 1);
        InitializePawns(Color.White, 2);
        InitializePawns(Color.Black, 7);
        InitializeOffense(Color.Black, 8);
    }

    private void UpdateBoardState()
    {
        if(IsChecked(Color.White))
        {
            State = BoardState.CheckWhite;
            Piece whiteKing = active_pieces.Values.FirstOrDefault(x => x is King && x.Color == Color.White);
            if(whiteKing.CandidateMoves(this).Count == 0)
                State = BoardState.CheckMateWhite;
            return;
        }

        if(IsChecked(Color.Black))
        {
            State = BoardState.CheckBlack;
            Piece whiteKing = active_pieces.Values.FirstOrDefault(x => x is King && x.Color == Color.Black);
            if(whiteKing.CandidateMoves(this).Count == 0)
                State = BoardState.CheckMateBlack;
            return;            
        }

        State = BoardState.Normal;
    }

    private Dictionary<CellPosition, Piece> active_pieces = new Dictionary<CellPosition, Piece>();
    private List<Piece> dead_pieces = new List<Piece>();
}

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
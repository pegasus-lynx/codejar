#load "enums.csx"
#load "cell.csx"
#load "models.csx"
#load "pieces.csx"
#load "board.csx"
#load "game.csx"

public class BoardPrinter : IBoardObserver
{
	public BoardPrinter(ChessBoard board)
	{
		_board = board;
		_board.Subscribe(this);
		InitialieCellStrings();
	}

	~BoardPrinter()
	{
		_board.Unsubscribe(this);
	}

	public void MoveMade(Move move)
	{
		Position from = move.From;
		Position to = move.To;

		_cellStrings[to.Row, to.Col] = _cellStrings[from.Row, from.Col];
		_cellStrings[from.Row, from.Col] = " ";
	}

    public void Print()
    {
        StringBuilder boardBuilder = new();
        boardBuilder.AppendLine("    A B C D E F G H");

        for(int r=8; r>0; r--)
        {
            boardBuilder.Append($"{r,2}  ");

        	for(int c=1; c<=8; c++)
        		boardBuilder.Append($"{_cellStrings[r,c]} ");

            boardBuilder.AppendLine($" {r}");
        }

        boardBuilder.AppendLine("    A B C D E F G H");
        Console.Write(boardBuilder);
    }

    private void InitialieCellStrings()
    {
    	for(int r=1;r<=8;r++)
    		for(int c=1;c<=8;c++)
    			_cellStrings[r,c] = " ";

    	foreach(Piece p in _board.GetPieces())
    	{
    		Position pos = p.Position;
    		_cellStrings[pos.Row, pos.Col] = p.GetSymbol();
    	}
    }

    private string[,] _cellStrings = new string[9,9];
    private ChessBoard _board;
}

public class GamePrinter
{
	public GamePrinter(ChessGame game, ChessBoard board)
	{
		_game = game;
		_board = board;
		_boardPrinter = new BoardPrinter(board);
	}

	public void Print()
	{
	    Console.OutputEncoding = System.Text.Encoding.UTF8;

	    if(Console.IsOutputRedirected)
	        Console.Write("\u001b[2J\u001b[H");
	    else
	        Console.Clear();

	    Player pw = _game.PlayerWhite, pb = _game.PlayerBlack;
        Console.WriteLine($"{pw.User.Name} (White) vs {pb.User.Name} (Black)");
        Console.WriteLine($"Game: {_game.State} | Board: {_board.State}");

        if(_game.State == GameState.Finished)
        {
		    Player winner = _game.Winner;
            Console.WriteLine($"Winner: {winner.User.Name} ({winner.Color})");
        }

        _boardPrinter.Print();		
	}


	private ChessGame _game;
	private ChessBoard _board;
	private BoardPrinter _boardPrinter;
}
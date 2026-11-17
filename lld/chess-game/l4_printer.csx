#load "l1_enums.csx"
#load "l1_position.csx"
#load "l1_models.csx"
#load "l2_pieces.csx"
#load "l3_board.csx"
#load "l4_game.csx"


public class GamePrinter
{
	public GamePrinter(ChessGame game, IBoardView boardView)
	{
		_game = game;
		_boardView = boardView;
	}

	public void Print()
	{
	    Console.OutputEncoding = System.Text.Encoding.UTF8;

	    if(Console.IsOutputRedirected)
	        Console.Write("\u001b[2J\u001b[H");
	    else
	        Console.Clear();

        PrintUsers();
        PrintGameState();
        PrintBoard();		
	}

	private void PrintUsers()
	{
	    Player pw = _game.PlayerWhite, pb = _game.PlayerBlack;
        Console.WriteLine($"{pw.ToString()} vs {pb.ToString()}");
	}

	private void PrintGameState()
	{
        Console.WriteLine($"Game: {_game.State}");
        if(_game.IsGameOver && _game.Winner is Player winner)
            Console.WriteLine($"Winner: {winner.ToString()}");
	}

    private void PrintBoard()
    {
        StringBuilder boardBuilder = new();
        boardBuilder.AppendLine("    A B C D E F G H");

        for(int r=7; r>=0; r--)
        {
            boardBuilder.Append($"{r+1,2}  ");
        	for(int c=0; c<8; c++)
        	{
            	PieceInfo pInfo = _boardView.GetAt(r,c);
        		boardBuilder.Append($"{pInfo?.Symbol ?? " "} ");
        	}

            boardBuilder.AppendLine($" {r+1}");
        }

        boardBuilder.AppendLine("    A B C D E F G H");
        Console.Write(boardBuilder);
    }

	private ChessGame _game;
	private IBoardView _boardView;
}
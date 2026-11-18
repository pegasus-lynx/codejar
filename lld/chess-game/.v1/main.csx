#load "models\enums.csx"
#load "models\models.csx"
#load "models\pieces.csx"
#load "models\entities.csx"


User u1 = new User("Alice");
User u2 = new User("Bob");

ChessGame game = new ChessGame(u1, u2);
PrintGame(game);

List<(string fromCell, string toCell)> turns = [
    ("E2", "E3"),
    ("E7", "E6"),
    ("E3", "E4"),
    ("E6", "E5"),
    ("D1", "F3"),
    ("F8", "E7"),
    ("F1", "C4"),
    ("G8", "H6"),
    ("D2", "D3"),
    ("E7", "G5"),
    ("C1", "G5"),
    ("D8", "G5"),
    ("G1", "E2"),
    ("D7", "D6")
];

foreach (var (fromCell, toCell) in turns)
{
    if(!game.TakeTurn(fromCell, toCell))
    {
        Console.WriteLine("Invalid Turn");
        return;
    }

    PrintGame(game);
    System.Threading.Thread.Sleep(500);
}


void PrintGame(ChessGame chessGame)
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;

    if(Console.IsOutputRedirected)
        Console.Write("\u001b[2J\u001b[H");
    else
        Console.Clear();

    chessGame.Print();
}


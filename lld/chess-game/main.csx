#load "l1_enums.csx"
#load "l1_models.csx"
#load "l4_game.csx"
#load "l4_printer.csx"

User u1 = new User("Alice");
User u2 = new User("Bob");

ChessGame game = new ChessGame(u1, u2);
GamePrinter printer = new GamePrinter(game, game.GetBoardView());
printer.Print();

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
    MoveError error = game.TakeTurn(fromCell, toCell);
    if(error != MoveError.None)
    {
        Console.WriteLine($"Invalid Turn - {error}");
        return;
    }

    printer.Print();
    System.Threading.Thread.Sleep(500);
}





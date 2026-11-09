
// TicTacToe Game
//  1. A game consists of 2 players
//  2. Each game consists of a board
//  3. Each board can be of size N ( deafult 3 )
//  4. Players take alternate moves.
//  5. Game can end in - WIN Player X, WIN Player Y, DRAW
//  6. Any cell in the game can be either empty, filled with X or an O

#load "tictactoe.enums.csx"
#load "tictactoe.exceptions.csx"
#load "tictactoe.entities.csx"

Player pa = new Player("Alice");
Player pb = new Player("Bob");

// Game 1
TicTacToeGame game1 = new TicTacToeGame(pa, pb, board_size: 3);
List<(int r, int c)> moves1 = new List<(int r, int c)>{ 
        (0,0), (0,1), (1,1), 
        (2,2), (1,0), (1,2), 
        (2,0) 
    };
foreach(var (r, c) in moves1)
    game1.MakeMove(r,c);

Console.WriteLine($"{game1.Winner().Name} == Alice");
Console.WriteLine($"{game1.State} == GameState.WIN_X");


// Game 2
TicTacToeGame game2 = new TicTacToeGame(pa, pb, board_size: 3);
List<(int r, int c)> moves2 = new List<(int r, int c)>{ 
        (0,0), (1,1), (1,0), 
        (2,0), (0,2), (0,1), 
        (2,1), (1,2), (2,2) 
    };
foreach(var (r, c) in moves2)
    game2.MakeMove(r,c);

Console.WriteLine($"{game2.Winner()} == null");
Console.WriteLine($"{game2.State} == GameState.DRAW");
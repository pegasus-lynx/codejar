#load "models\enums.csx"
#load "models\models.csx"
#load "models\pieces.csx"
#load "models\entities.csx"


User u1 = new User("Alice");
User u2 = new User("Bob");

ChessGame game = new ChessGame(u1, u2);
game.TakeTurn("E2", "E3");
game.TakeTurn("E7", "E6");
game.TakeTurn("E3", "E4");
game.TakeTurn("E6", "E5");
game.TakeTurn("D1", "D3");
game.TakeTurn("F8", "E7");
game.TakeTurn("F1", "F4");
game.TakeTurn("G8", "G6");
game.TakeTurn("D2", "D3");
game.TakeTurn("E7", "G5");
game.TakeTurn("C1", "G5");
game.TakeTurn("D8", "G5");
game.TakeTurn("G1", "E2");
game.TakeTurn("D7", "D6");
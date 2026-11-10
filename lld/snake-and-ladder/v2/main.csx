
#load "entities.csx"


List<Player> players = new List<Player>();
players.Add(new Player("Angela"));
players.Add(new Player("Boyle"));
players.Add(new Player("Cho"));
players.Add(new Player("Draco"));


List<Ladder> ladders = new List<Ladder>();
ladders.Add(new Ladder(5, 15));
ladders.Add(new Ladder(18, 45));
ladders.Add(new Ladder(27, 34));
ladders.Add(new Ladder(42, 63));
ladders.Add(new Ladder(61, 89));
ladders.Add(new Ladder(73, 98));

List<Snake> snakes = new List<Snake>();
snakes.Add(new Snake(31, 6));
snakes.Add(new Snake(93, 54));
snakes.Add(new Snake(83, 64));
snakes.Add(new Snake(72, 66));
snakes.Add(new Snake(78, 44));
snakes.Add(new Snake(46, 12));

var game = new SnakeAndLadderGame(players, 10, ladders, snakes);
game.Play();
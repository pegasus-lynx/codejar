
public enum GameState
{
	NotStarted,
	InProgress,
	Finished
}

public abstract class BoardTunnel
{
	public BoardTunnel(int start, int end)
	{
		if(start <= 0 || end <= 0)
			throw new ArgumentException($"start:{start}, end:{end} should be positive");
		Start = start;
		End = end;
	}

	public override string ToString() => $"{Tag} (Start:{Start}, End:{End})";

	public int Start { get; private set; }
	public int End { get; private set; }
	protected abstract string Tag { get; }
}

public class Ladder: BoardTunnel
{
	public Ladder(int start, int end) : base(start, end)
	{
		if(start >= end)
			throw new ArgumentException($"In ladder, start < end. start={start}, end={end}");
	}

	protected override string Tag => "Ladder";
}

public class Snake: BoardTunnel
{
	public Snake(int start, int end) : base(start, end)
	{
		if(start <= end)
			throw new ArgumentException($"In snake, start > end. start={start}, end={end}");
	}

	protected override string Tag => "Snake";
}

public class Player
{
	public Player(string name) { Name = name; }
	public string Name { get; private set; }
}

public class GamePlayer
{
	public GamePlayer(Player player) { Player = player; }

	public Player Player { get; private set; }
	public int Position 
	{ 
		get => position; 
		set
		{
			if(value < 1)
				throw new ArgumentException($"Position should be positive ({value})");
			position = value;
		}
	}

	private int position = 1;
}

public interface IDice
{
	int Roll();
}

public class Dice : IDice
{
	public Dice(int minValue=1, int maxValue=6)
	{
		this.minValue = minValue;
		this.maxValue = maxValue;
	}

	public int Roll() { return random.Next(minValue,maxValue+1); }

	private readonly Random random = new Random();
	private readonly int minValue;
	private readonly int maxValue;
}

public class Board
{
	public Board(int size, List<Ladder> ladders, List<Snake> snakes)
	{
		if(size < 0)
			throw new ArgumentException("Board size cannot be negative.");
		
		this.finalPosition = size * size;

		foreach(Ladder l in ladders)
			if(!AddTunnel(l))
				throw new ArgumentException("Another ladder/snake starts at the same position.");

		foreach(Snake s in snakes)
			if(!AddTunnel(s))
				throw new ArgumentException("Another ladder/snake starts at the same position.");
	}

	public int GetFinalPosition(int currentPos, int shift)
	{
		int newPos = currentPos + shift;
		if(newPos > finalPosition)
			return currentPos;

		while(true)
		{
			if(!tunnels.ContainsKey(newPos))
				break;

			newPos = tunnels[newPos].End;
			if(newPos == (currentPos+shift))
				throw new Exception("There is a cycle in the board.");
		}

		return newPos;
	}

	public bool IsEndPosition(int pos) => pos == finalPosition;

	public bool AddTunnel(BoardTunnel tunnel)
	{
		if(tunnels.ContainsKey(tunnel.Start))
			return false;

		tunnels[tunnel.Start] = tunnel;
		return true;
	}

	private bool ValidPosition(int p) => p > 0 && p < finalPosition;

	private Dictionary<int, BoardTunnel> tunnels = new Dictionary<int, BoardTunnel>();
	private int finalPosition;
}

public class SnakeAndLadderGame
{
	public SnakeAndLadderGame(List<Player> players, int size, List<Ladder> ladders, List<Snake> snakes)
	{
		foreach(Player p in players)
			this.gamePlayers.Add(new GamePlayer(p));
		this.board = new Board(size, ladders, snakes);
	}

	public void Play()
	{
		State = GameState.InProgress;
		while(State == GameState.InProgress)
		{
			TakeTurn();
			Console.WriteLine(GetGameStatus());
		}

		Console.WriteLine($"{Winner.Name} won the game!");
	}

	public void TakeTurn()
	{
		if(State == GameState.Finished)
			throw new Exception("The game already finished.");

		GamePlayer p = gamePlayers[nextPlayer];
		int move = GetCurrentRoll();

		p.Position = board.GetFinalPosition(p.Position, move);
		if(board.IsEndPosition(p.Position))
		{
			State = GameState.Finished;
			Winner = gamePlayers[nextPlayer].Player;
			return;
		}

		nextPlayer = (nextPlayer + 1) % gamePlayers.Count;	
	}

	public string GetGameStatus()
	{
		string positions = "Positions: "; 
		for(int i=0; i<gamePlayers.Count; i++)
		{
			positions += $"Player {i} - {gamePlayers[i].Position}, ";
		}
		positions += $"State: {State}";
		return positions;
	}

	private int GetCurrentRoll()
	{
		int roll = 0;
		int roll_sum = 0;
		int turns = 0;

		while(true)
		{
			roll = dice.Roll();
			roll_sum += roll;
			turns += 1;

			if(turns == 3 || roll != 6)
			{
				if(roll_sum == 18)
					roll_sum = 0;
				break;
			}
		}

		return roll_sum;
	}

	public GameState State { get; private set; } = GameState.NotStarted;
	public Player Winner { get; private set; }

	private Board board;
	private readonly List<GamePlayer> gamePlayers = new List<GamePlayer>();
	private int nextPlayer = 0;
	private IDice dice = new Dice();
}
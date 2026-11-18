
public enum GameState
{
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

	public override string ToString()
	{
		return $"Start:{Start}, End:{End}";
	}

	public int Start { get; private set; }
	public int End { get; private set; }
}

public class Ladder: BoardTunnel
{
	public Ladder(int start, int end) : base(start, end)
	{
		if(start >= end)
			throw new ArgumentException($"In ladder, start < end. start={start}, end={end}");
	}

	public override string ToString()
	{
		return $"Ladder ({base.ToString()})";
	}
}

public class Snake: BoardTunnel
{
	public Snake(int start, int end) : base(start, end)
	{
		if(start <= end)
			throw new ArgumentException($"In snake, start > end. start={start}, end={end}");
	}

	public override string ToString()
	{
		return $"Snake ({base.ToString()})";
	}
}

public class Player
{
	public Player(string name) 
	{ 
		Name = name;
		Position = 1; 
	}

	public void SetPosition(int pos)
	{
		if(pos < 1)
			throw new ArgumentException($"Position should be positive ({pos})");

		Position = pos;
	}

	public string Name { get; private set; }
	public int Position { get; private set; }
}

public class Dice
{
	public int Roll() { return random.Next(1,7); }

	private Random random = new Random();
}

public class Board
{
	public Board(int size, List<Ladder> ladders, List<Snake> snakes)
	{
		if(size < 0)
			throw new ArgumentException("Board size cannot be negative.");
		
		this.board_size = size * size;
		this.ladders = new List<Ladder>();
		this.snakes = new List<Snake>();

		HashSet<int> starts = new HashSet<int>();

		foreach(Ladder l in ladders)
			if(ValidateTunnel(l, starts))
				this.ladders.Add(l);

		foreach(Snake s in snakes)
			if(ValidateTunnel(s, starts))
				this.snakes.Add(s);
	}

	public int GetNewPosition(int currentPos, int shift)
	{
		if(currentPos + shift > board_size)
			return currentPos;

		int newPos = currentPos+shift;
		BoardTunnel tunnel = MatchesAny(newPos);
		while(tunnel is not null)
		{
			newPos = tunnel.End;
			if(newPos == (currentPos+shift))
				throw new Exception("There is a cycle in the board.");
			tunnel = MatchesAny(newPos);
		}

		return newPos;
	}

	public bool IsEndPosition(int pos) { return pos == board_size; }

	private BoardTunnel MatchesAny(int pos)
	{
		foreach(Ladder l in ladders)
			if(l.Start == pos)
				return l;

		foreach(Snake s in snakes)
			if(s.Start == pos)
				return s;

		return null;
	}

	private bool ValidateTunnel(BoardTunnel tunnel, HashSet<int> starts)
	{
		if(!ValidPosition(tunnel.Start) || !ValidPosition(tunnel.End))
			throw new ArgumentException($"{tunnel.ToString()} is not valid for board.");

		if(starts.Contains(tunnel.Start))
			throw new ArgumentException("Two ladder/snakes have the same start position.");

		starts.Add(tunnel.Start);
		return true;
	}

	private bool ValidPosition(int p) { return p >= 0 && p < board_size; }

	private List<Ladder> ladders;
	private List<Snake> snakes;
	private int board_size;
}

public class SnakeAndLadderGame
{
	public SnakeAndLadderGame(List<Player> players, int size, List<Ladder> ladders, List<Snake> snakes)
	{
		this.players = players;
		this.board = new Board(size, ladders, snakes);
	}

	public bool MakeMove()
	{
		if(State == GameState.Finished)
			throw new Exception("The game already finished.");

		Player p = players[nextPlayer];
		int shift = GetCurrentRoll();
		int newPos = board.GetNewPosition(p.Position, shift);
		p.SetPosition(newPos);
		nextPlayer = (nextPlayer + 1) % players.Count;	
		if(board.IsEndPosition(newPos))
		{
			State = GameState.Finished;
			return true;
		}
		return false;
	}

	public string GetGameStatus()
	{
		string positions = "Positions: "; 
		for(int i=0; i<players.Count; i++)
		{
			positions += $"Player {i} - {players[i].Position}, ";
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

	public GameState State { get; private set; } = GameState.InProgress;
	public Player Winner { get; private set; }

	private Board board;
	private List<Player> players;
	private int nextPlayer = 0;
	private Dice dice = new Dice();
}
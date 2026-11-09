#load "tictactoe.enums.csx"
#load "tictactoe.exceptions.csx"

class Player
{
	public Player(string name) { Name = name; }
	public string Name { get; private set; }
}

class Cell
{
	public CellSymbol Symbol { get; private set; }

	public bool SetCellCymbol(CellSymbol symbol)
	{
		if(!IsEmpty() || symbol == CellSymbol.EMPTY)
			return false;

		Symbol = symbol;
		return true;
	}

	public bool IsEmpty()
	{
		return Symbol == CellSymbol.EMPTY;
	}
}

class Board
{
	public Board(int board_size)
	{
		if(board_size < 1)
			throw new ArgumentException($"Board size should be positive: {board_size}");
		size = board_size;
		InitializeBoard();
	}

	public int Size { get{ return size; } }

	public void MarkCell(CellSymbol symbol, int row, int col)
	{
		if(!IsValidCell(row, col))
			throw new InvalidMoveException($"Position ({row}, {col}) is out of bounds");

		if(!grid[row,col].SetCellCymbol(symbol))
			throw new InvalidMoveException($"Cell ({row}, {col}) is already occupied");

		marked_count++;
	}

	public bool IsFull()
	{
		return marked_count == (size*size);
	}

	private void InitializeBoard()
	{
		grid = new Cell[size, size];
		for(int i=0;i<size;i++)
			for(int j=0;j<size;j++)
				grid[i,j] = new Cell();
	}

	private bool IsValidCell(int row, int col)
	{
		return row >= 0 && col >= 0 && row < size && col < size;
	}

	public (int nx, int no) GetColumnSymbolCounts(int c) => GetSymbolCounts(0, c, 1, 0);
	public (int nx, int no) GetRowSymbolCounts(int r) => GetSymbolCounts(r, 0, 0, 1);
	public (int nx, int no) GetDiagonalSymbolCounts(bool reverse=false)
	{
		int r = 0, c = reverse ? size-1 : 0;
		int dr = 1, dc = reverse ? -1 : 1;
		return GetSymbolCounts(r, c, dr, dc);
	}

	private (int nx, int no) GetSymbolCounts(int x, int y, int dx, int dy)
	{
		int nx=0, no=0;
		for(int i=0; i<size; i++)
		{
			int r = x + (dx*i);
			int c = y + (dy*i);
			CellSymbol symbol = grid[r,c].Symbol;
			
			if(symbol == CellSymbol.X)
				nx++;
			else if(symbol == CellSymbol.O)
				no++;
		}

		return (nx, no);
	}

	private Cell[,] grid;
	private int size;
	private int marked_count = 0;
}


class TicTacToeGame
{
	public TicTacToeGame(Player x, Player o, int board_size)
	{
		if(x is null || o is null)
			throw new ArgumentException("Players cannot be null");
		players = new Player[] {x, o};
		board = new Board(board_size);
	}

	public void MakeMove(int r, int c)
	{
		if(State != GameState.STARTED)
			throw new InvalidMoveException("Game has already over!");

		CellSymbol symbol = nextPlayer == 0 ? CellSymbol.X : CellSymbol.O;
		
		board.MarkCell(symbol, r, c);
		UpdateGameState(r, c);
		nextPlayer = (nextPlayer+1) % 2;
	}

	public Player Winner()
	{
		if(State == GameState.WIN_O)
			return PlayerO;

		if(State == GameState.WIN_X)
			return PlayerX;

		return null;
	}

	private void UpdateGameState(int r, int c)
	{
		int nx, no;

		(nx,no) = board.GetRowSymbolCounts(r);
		if(SetGameState(nx,no)) 
			return;

		(nx,no) = board.GetColumnSymbolCounts(c);
		if(SetGameState(nx,no)) 
			return;

		if(r==c)
		{
			(nx,no) = board.GetDiagonalSymbolCounts();
			if(SetGameState(nx,no)) 
				return;
		}

		if(r+c == board.Size-1)
		{
			(nx,no) = board.GetDiagonalSymbolCounts(reverse: true);
			if(SetGameState(nx,no)) 
				return;
		}

		if(board.IsFull())
			State = GameState.DRAW;
	}

	private bool SetGameState(int nx, int no)
	{
		if(nx == board.Size)
		{
			State = GameState.WIN_X;
			return true;
		}

		if(no == board.Size)
		{
			State = GameState.WIN_O;
			return true;
		}
	
		return false;
	}

	public Player PlayerX { get{ return players[0]; } }
	public Player PlayerO { get{ return players[1]; } }
	public GameState State { get; private set; }

	private int nextPlayer = 0;
	private Player[] players;
	private Board board;
}
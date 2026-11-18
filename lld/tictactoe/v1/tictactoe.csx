
// TicTacToe Game
//  1. A game consists of 2 players
//  2. Each game consists of a board
//  3. Each board can be of size N ( deafult 3 )
//  4. Players take alternate moves.
//  5. Game can end in - WIN Player X, WIN Player Y, DRAW
//  6. Any cell in the game can be either empty, filled with X or an O

#load "tictactoe.enums.csx"

using System;

class Player
{
    public Player(string name)
    {
        Name = name;
    }

    public string Name {get; set;}
}

class TicTacToeCell
{
    public CellSymbol CellState { get { return cellState; } }
    
    public bool TrySetCellState(CellSymbol state)
    {
        if(IsFilled() || state == CellSymbol.EMPTY)
            return false;

        cellState = state;
        return true;
    }

    public bool IsFilled()
    {
        return cellState != CellSymbol.EMPTY;
    }

    private CellSymbol cellState;
}

class TicTacToeBoard
{  
    public TicTacToeBoard(int size)
    {
        board_size = size;
        InitializeBoard();
    }


    public bool MarkCell(CellSymbol state, int cellRow, int cellColumn)
    {
        int cellIndex = (board_size*cellRow) + cellColumn;
        
        if(cellIndex<0 || cellIndex>cells.Count)
            return false;

        TicTacToeCell cell = cells[cellIndex];
        if(!cell.TrySetCellState(state))
            return false;

        return true;
    }

    private void InitializeBoard()
    {
        cells = new List<TicTacToeCell>(board_size*board_size);
        for(int i=1; i<=board_size; i++)
        {
            for(int j=1; j<=board_size; j++)
            {
                cells.Add(new TicTacToeCell());
            }
        }
    }

    public (int nx, int ny) GetColumnSymbolCounts(int c) => GetSymbolCounts(0, c, 1, 0);
    public (int nx, int ny) GetRowSymbolCounts(int r) => GetSymbolCounts(r, 0, 0, 1);
    public (int nx, int ny) GetDiagSymbolCounts(bool reverse = false) => GetSymbolCounts(0, reverse ? board_size-1 : 0, 1, reverse ? -1 : 1);

    private (int nx, int ny) GetSymbolCounts(int x, int y, int dx, int dy)
    {
        int xs=0, ys=0;
        for(int i=0;i<board_size;i++)
        {
            int r = x + (dx*i);
            int c = y + (dy*i);

            int index = (board_size*r) + c;
            switch(cells[index].CellState)
            {
                case CellSymbol.X:
                    xs++;
                    break;
                case CellSymbol.O:
                    ys++;
                    break;
                default:
                    break;
            }
        }

        return (xs, ys);
    }

    private List<TicTacToeCell> cells;
    private int board_size = 3;
}

class TicTacToeGame
{
    public TicTacToeGame(Player x, Player o)
    {
        PlayerX = x;
        PlayerO = o;
        board = new TicTacToeBoard(board_size);
    }

    public Player PlayerX { get; private set; }
    public Player PlayerO { get; private set; }
    public GameState State { get; private set; }
    
    public bool MakeMove(Player p, int cellRow, int cellCol)
    {
        if(State != GameState.STARTED)
            return false;

        CellSymbol symbol = CellSymbol.EMPTY;

        if(p == PlayerX)
            symbol = CellSymbol.X;
        
        if(p == PlayerO)
            symbol = CellSymbol.O;

        if(board.MarkCell(symbol, cellRow, cellCol))
        {
            moves_count += 1;
            UpdateGameState();
            return true;
        }

        return false;
    }

    private void UpdateGameState()
    {
        var (xs, ys) = (0, 0);
        for(int r=0; r<board_size; r++)
        {
            (xs, ys) = board.GetRowSymbolCounts(r);
            if(SetGameState(xs, ys))
                return;
        }

        for(int c=0; c<board_size; c++)
        {
            (xs, ys) = board.GetColumnSymbolCounts(c);
            if(SetGameState(xs, ys))
                return;
        }

        (xs, ys) = board.GetDiagSymbolCounts();
        if(SetGameState(xs, ys))
            return;

        (xs, ys) = board.GetDiagSymbolCounts(reverse: true);
        if(SetGameState(xs, ys))
            return;

        if(moves_count == (board_size*board_size))
            State = GameState.DRAW;
    }

    private bool SetGameState(int nx, int no)
    {
        if(nx == board_size)
        {
            State = GameState.WIN_X;
            return true;
        }

        if(no == board_size)
        {
            State = GameState.WIN_O;
            return true;
        }

        return false;    
    }

    public Player Winner()
    {
        if(State == GameState.WIN_X)
            return PlayerX;
        
        if(State == GameState.WIN_O)
            return PlayerO;
        
        return null;
    }

    private TicTacToeBoard board;
    private int board_size = 3; 
    private int moves_count = 0;
}

Player px = new Player("Alice");
Player po = new Player("Bob");

TicTacToeGame game = new TicTacToeGame(px, po);
game.MakeMove(px, 0, 0);
game.MakeMove(po, 0, 1);
game.MakeMove(px, 1, 1);
game.MakeMove(po, 2, 2);
game.MakeMove(px, 1, 0);
game.MakeMove(po, 1, 2);
game.MakeMove(px, 2, 0);
game.MakeMove(po, 0, 2);

Console.WriteLine($"{game.Winner().Name} is the winner of the game.");
#load "enums.csx"

public class CellPosition
{
    public CellPosition(int row, int col)
    {
        Row = row;
        Col = col;    
    }

    public CellPosition(CellPosition other)
    {
        Row = other.Row;
        Col = other.Col;
    }

    public static CellPosition From(string cellCode)
    {
        int col = GetCol(cellCode[0]);
        int row = GetRow(cellCode[1]);
        if(row < 1 || row > 8 || col < 1 || col > 8)
            throw new ArgumentException($"Invalid Cell Code - {cellCode}");
        return new CellPosition(row, col); 
    }

    public string CellCode => $"{(char)('A' + (Col - 1))}{(char)('0' + Row)}";
    public override string ToString() => CellCode;

    public int Row { get; private set; }
    public int Col { get; private set; }

    private static int GetRow(char rowId) => rowId - '0';
    private static int GetCol(char colId) => (colId - 'A') + 1; 
}

public readonly record struct User(string Name);

public class Player
{
    public Player(User user, Color color)
    {
        this.User = user;
        this.Color = color;
    }

    public User User { get; private set; }
    public Color Color { get; private set; }
}

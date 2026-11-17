#load "enums.csx"

public record class Position
{
    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public static Position From(string code)
    {
        if(code is null || code.Length != 2)
            throw new ArgumentException($"Invalid Cell Code - {code}");

        int col = (code[0] - 'A') + 1;
        int row = code[1] - '0';
        
        return new Position(row, col); 
    }

    public static Position operator -(Position a, Position b)
    {
    	return new Position(a.Row - b.Row, a.Col - b.Col);
    }

    public static Position operator +(Position a, Position b)
    {
    	return new Position(a.Row + b.Row, a.Col + b.Col);
    }

    public static bool CheckBounds(int row, int col) => row > 0 && row < 9 && col > 0 && col < 9;

    public int Row { get; init; }
    public int Col { get; init; }

    public string Code => $"{(char)('A' + (Col - 1))}{(char)('0' + Row)}";
    public override string ToString() => Code;
}

public class Cell
{
    public Cell(Position position, Piece piece = null)
    {
        Position = position;
        Occupant = piece;
    }

    public Position Position { get; init; }
    public Piece Occupant {get; set;} = null;

    public bool IsOccupied() => Occupant is not null;
}
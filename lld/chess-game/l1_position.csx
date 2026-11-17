#load "l1_enums.csx"

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

        int col = (code[0] - 'A');
        int row = (code[1] - '0') - 1;

        if(!Position.CheckBounds(row, col))
            throw new ArgumentException($"Invalid Cell Code - {code}");
        
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

    public static bool CheckBounds(int row, int col) => row >= 0 && row < 8 && col >= 0 && col < 8;

    public int Row { get; init; }
    public int Col { get; init; }

    public override string ToString()
    {
        return $"{(char)('A' + (Col))}{(char)('0' + (Row+1))}";
    }
}
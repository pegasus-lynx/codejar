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

        if(row < 1 || row > 8 || col < 1 || col > 8)
            throw new ArgumentException($"Invalid Cell Code - {code}");
        
        return new Position(row, col); 
    }

    public int Row { get; init; }
    public int Col { get; init; }

    public string Code => $"{(char)('A' + (Col - 1))}{(char)('0' + Row)}";
    public override string ToString() => Code;
}

public readonly record struct User(string Name);

public readonly record class Player(User User, Color Color);
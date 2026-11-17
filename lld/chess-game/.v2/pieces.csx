#load "enums.csx"
#load "board.csx"
#load "cell.csx"
#load "move.csx"

public abstract class Piece
{
    public Piece(Position pos, Color color, MoveStrategy strategy = null) 
    { 
        Position = pos; 
        Color = color; 
        moveStrategy = strategy;
    }

    public Position Position { get; set; }
    public Color Color { get; init; }
    public abstract string GetSymbol();

    public bool CanReach(Position to, ChessBoard board)
    {
        return moveStrategy.CanReach(Position, to, board);
    }

    protected MoveStrategy moveStrategy;
}

public class Pawn : Piece
{
    public Pawn(Position pos, Color color) : base(pos, color, new PawnStrategy()) {} 

    public override string GetSymbol()
    {
        return Color == Color.White ? "P" : "p";
    }

}

public class King : Piece
{
    public King(Position pos, Color color) : base(pos, color, new KingStrategy()) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "K" : "k";
    }

}

public class Queen : Piece
{
    public Queen(Position pos, Color color) : base(pos, color, new QueenStrategy()) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "Q" : "q";
    }

}

public class Knight : Piece
{
    public Knight(Position pos, Color color) : base(pos, color, new KnightStrategy()) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "N" : "n";
    }

}

public class Bishop : Piece
{
    public Bishop(Position pos, Color color) : base(pos, color, new BishopStrategy()) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "B" : "b";
    }

}

public class Rook : Piece
{
    public Rook(Position pos, Color color) : base(pos, color, new RookStrategy()) {}

    public override string GetSymbol()
    {
        return Color == Color.White ? "R" : "r";
    }

}
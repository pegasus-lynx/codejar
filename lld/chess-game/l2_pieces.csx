#load "l1_enums.csx"
#load "l1_position.csx"
#load "l2_move.csx"
#load "l3_board.csx"

public record class PieceInfo(string Symbol, Color Color);

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
    public virtual bool Jumps => false;
    protected abstract char Letter { get; }

    public string Symbol => Color == Color.White ? Letter.ToString()
                                               : char.ToLower(Letter).ToString();

    public bool CanReach(Position to, ChessBoard board)
    {
        return moveStrategy.CanReach(Position, to, board);
    }

    protected MoveStrategy moveStrategy;
}

public class Pawn : Piece
{
    public Pawn(Position pos, Color color) : base(pos, color, new PawnStrategy()) {} 
    protected override char Letter => 'P';
}

public class King : Piece
{
    public King(Position pos, Color color) : base(pos, color, new KingStrategy()) {}
    protected override char Letter => 'K';
}

public class Queen : Piece
{
    public Queen(Position pos, Color color) : base(pos, color, new QueenStrategy()) {}
    protected override char Letter => 'Q';
}

public class Knight : Piece
{
    public Knight(Position pos, Color color) : base(pos, color, new KnightStrategy()) {}
    protected override char Letter => 'N';
    public override bool Jumps => true;
}

public class Bishop : Piece
{
    public Bishop(Position pos, Color color) : base(pos, color, new BishopStrategy()) {}
    protected override char Letter => 'B';
}

public class Rook : Piece
{
    public Rook(Position pos, Color color) : base(pos, color, new RookStrategy()) {}
    protected override char Letter => 'R';
}

public enum Color
{
    White,
    Black
}

public static Color Opposite(this Color c) => c == Color.White ? Color.Black : Color.White;

public enum MoveError
{
    None,
    InvalidPosition,
    SameSquare,             // from == to
    WrongColor,             // piece belongs to other side  
    NoPiece,                // empty source
    OwnPieceAtTarget,       // own piece occupies the target
    Unreachable,            // cell is unreachable by the piece 
    PathBlocked,          // path is blocked by some other piece
    LeavesKingInCheck,      // move results in check
    DoesNotResolveCheck,    // cannot move because in check
    GameOver                // game is over
}

public enum GameState
{
    Ongoing,
    Check,
    CheckMate,
    StaleMate
}

public enum Color
{
    White,
    Black
}

public enum BoardState
{
    None,
    Check,
    CheckMate,
    StaleMate
}

public enum MoveError
{
    None,
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
    Started,
    InProgress,
    Finished
}

public enum MoveKind
{
    Capture,
    EnPassant,
    Castling
}
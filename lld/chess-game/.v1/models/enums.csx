
public enum Color
{
    White,
    Black
}

public enum BoardState
{
    Normal,
    CheckBlack,
    CheckWhite,
    CheckMateBlack,
    CheckMateWhite
}

public enum MoveResult
{
    Succeeded,
    EmptyStart,  // Done
    TargetOccupied, // Done
    Unreachable, // Done
    ExposesKing // Done
}

public enum GameState
{
    InProgress,
    Finished
}
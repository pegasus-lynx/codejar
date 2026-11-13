
public enum Color
{
    White,
    Black
}

public enum BoardState
{
    None,
    StaleMate,
    CheckWhite,
    CheckBlack,
    CheckMateWhite,
    CheckMateBlack
}

public enum MoveResult
{
    Valid,
    EmptyStart,  
    TargetOccupied, 
    Unreachable, 
    ExposesKing 
}

public enum MoveState
{
    
}

public enum GameState
{
    InProgress,
    Finished
}
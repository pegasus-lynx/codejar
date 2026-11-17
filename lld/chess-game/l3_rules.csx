#load "l1_enums.csx"
#load "l1_position.csx"
#load "l2_pieces.csx"
#load "l3_board.csx"

public class RulesEngine
{
	public RulesEngine(ChessBoard board) { this.board = board; }

	public MoveError Validate(Piece piece, Position to)
	{
		if(piece.Position == to)
			return MoveError.SameSquare;

		if(!IsEmpty(to) && board.GetPiece(to).Color == piece.Color)
			return MoveError.OwnPieceAtTarget;

		if(!piece.CanReach(to, board))
			return MoveError.Unreachable;

		if(!piece.Jumps && IsPathBlocked(piece, to))
			return MoveError.PathBlocked;

		if(MoveResultsInCheck(piece, to))
		{
			if(IsInCheck(piece.Color))
				return MoveError.DoesNotResolveCheck;
			return MoveError.LeavesKingInCheck;	
		}

		return MoveError.None;

	}

	public GameState EvaluateGameState(Color sideToMove)
	{
		if(IsInCheckMate(sideToMove))
			return GameState.CheckMate;

		if(IsInCheck(sideToMove))
			return GameState.Check;

		if(IsInStaleMate(sideToMove))
			return GameState.StaleMate;

		return GameState.Ongoing;
	}

	public bool IsInCheck(Color sideToMove)
	{
		Piece king = FindKing(sideToMove);
		return board.GetPieces(sideToMove.Opposite())
					.Any(p => CanAttack(p, king.Position));
	}

	public bool IsInCheckMate(Color sideToMove)
	{
		return IsInCheck(sideToMove) && !HasLegalMove(sideToMove);
	}

	public bool IsInStaleMate(Color sideToMove)
	{
		return !IsInCheck(sideToMove) && !HasLegalMove(sideToMove);
	}

	// Private Methods and Members
	private bool MoveResultsInCheck(Piece piece, Position to)
	{
		bool result = false;
		Move move = board.Move(piece.Position, to);	
		try
		{
			result = IsInCheck(piece.Color);
		}
		finally
		{
			board.UndoMove(move);		
		}

		return result;
	}

	private bool HasLegalMove(Color color) 
	{
		return board.GetPieces(color).Any(p => CanMove(p));
	}

	private bool CanMove(Piece p)
	{
		for(int r=0;r<8;r++)
		{
			for(int c=0;c<8;c++)
			{
				Position pos = new(r, c);
				if(Validate(p, pos) == MoveError.None)
					return true;
			}
		}

		return false;
	}

	private bool CanAttack(Piece p, Position pos)
	{
		return p.CanReach(pos, board) && (p.Jumps || !IsPathBlocked(p, pos));
	}
	
	private bool IsPathBlocked(Piece p, Position to)
	{
		Position del = to - p.Position;
		Position step = new(Math.Sign(del.Row), Math.Sign(del.Col));

		for(Position pos = p.Position + step; pos != to; pos += step)
	        if (!IsEmpty(pos))
	            return true;
		
		return false;
	}

	private King FindKing(Color color)
	{
		return board.GetPieces(color).First(p => p is King) as King;
	}

	private bool IsEmpty(Position pos) => board.GetPiece(pos) is null;

	private ChessBoard board;
}
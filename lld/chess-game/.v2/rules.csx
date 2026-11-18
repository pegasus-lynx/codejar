#load "enums.csx"
#load "cell.csx"
#load "pieces.csx"
#load "board.csx"

public class RulesEngine
{
	public RulesEngine(ChessBoard board) { _board = board; }

	public MoveError ValidateMove(Position from, Position to)
	{
		if(from == to)
			return MoveError.SameSquare;

		if(_board.GetPiece(from) is not Piece fromPiece)
			return MoveError.NoPiece;

		if(_board.GetPiece(to) is Piece toPiece && !ColorDiffers(fromPiece, toPiece))
			return MoveError.OwnPieceAtTarget;

		if(!fromPiece.CanReach(to, _board))
			return MoveError.Unreachable;

		if(!CanReachPosition(fromPiece, to))
			return MoveError.PathBlocked;

		if(MoveResultsInCheck(fromPiece, to))
		{
			if(_board.State == BoardState.Check)
				return MoveError.DoesNotResolveCheck;
			return MoveError.LeavesKingInCheck;	
		}

		return MoveError.None;
	}

	public bool IsInStaleMate(Color color)
	{
	    if (IsInCheck(color) || CanKingMove(color))
	        return false;

	    foreach (Piece piece in _board.GetPieces())
	        if (piece.Color == color && piece is not King)
	            if (CanPieceMove(piece))
	                return false;

	    return true;
	}

	public bool IsInCheckMate(Color color)
	{
		return IsInCheck(color) && !CanKingMove(color);
	}

	public bool IsInCheck(Color color)
	{
		King king = FindKing(color);

		foreach(Piece piece in _board.GetPieces())
			if(ColorDiffers(king, piece) && piece is not King)
				if(CanReachPosition(piece, king.Position))
					return true;

		return false;
	}

	// Private Methods and Members
	private bool IsPathClear(Position from, Position to)
	{
		if(!TryGetDirection(from, to, out Position dir))
			return false;

		Position pos = from + dir;
		while(pos != to)
		{
			if(_stagedMove is not null && _stagedMove.To == pos)
				return false;

			if(_board.GetPiece(pos) is not null)
				return false;

			if(_stagedMove is not null && _stagedMove.From == pos)
				return false;	
		
			pos = pos+dir;
		}

		return true;
	}

	private King FindKing(Color color)
	{
		foreach(Piece piece in _board.GetPieces())
			if(piece is King king && piece.Color == color)
				return king;

		return null;
	}

	private bool CanKingMove(Color color)
	{
		King king = FindKing(color);
		King otherKing = FindKing(GetOtherColor(king));
	
		for(int r=1; r<=8; r++)
		{
			for(int c=1; c<=8; c++)
			{
				Position pos = new(r,c);
				if(CanKingReachPosition(king, pos))
					return true;
			}
		}

		return false;
	}

	private bool CanPieceMove(Piece p)
	{
		for(int r=1; r<=8; r++)
		{
			for(int c=1; c<=8; c++)
			{
				Position pos = new(r,c);
				if(CanPieceReachPosition(p, pos))
					return true;
			}
		}

		return false;
	}

	private bool CanColorCapturePosition(Color color, Position pos)
	{
		foreach(Piece piece in _board.GetPieces())
			if(piece.Color != color && CanReachPosition(piece, pos))
				return true;

		return false;
	}

	private bool CanReachPosition(Piece p, Position to)
	{
		if(p is King k)
			return CanKingReachPosition(k, to);
		return CanPieceReachPosition(p, to);
	}

	private bool CanPieceReachPosition(Piece p, Position to)
	{
		return p.CanReach(to, _board) && (p is Knight || IsPathClear(p.Position, to));
	}

	private bool CanKingReachPosition(King k, Position to)
	{
		King ok = FindKing(GetOtherColor(k));
		return CanPieceReachPosition(k, to) && !CanPieceReachPosition(ok, to);
	}

	private bool TryGetDirection(Position from, Position to, out Position dir)
	{
		dir = new(0,0);
		Position diff = to - from;
		int dr = diff.Row, dc = diff.Col;

		if(dr == 0 && dc == 0)
			return false;

		bool straight = dr == 0 || dc == 0;
		bool diagonal = Math.Abs(dr) == Math.Abs(dc);

		if(!straight && !diagonal)
			return false;

		if(dr != 0) dr /= Math.Abs(dr);
		if(dc != 0) dc /= Math.Abs(dc);
		dir = new Position(dr, dc);
		return true;
	}

	private bool MoveResultsInCheck(Piece fromPiece, Position to)
	{
		_stagedMove = new Move() { From = fromPiece.Position, To = to};
		bool result = IsInCheck(fromPiece.Color);
		_stagedMove = null;
		return result;
	}

	private bool IsPositionEmpty(Position pos) => _board.GetPiece(pos) is null;

	private bool ColorDiffers(Piece p1, Piece p2) => p1.Color != p2.Color;

	private Color GetOtherColor(Piece p) => p.Color == Color.White ? Color.Black : Color.White;

	private ChessBoard _board;
	private Move _stagedMove;
}
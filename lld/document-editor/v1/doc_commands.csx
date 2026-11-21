
public interface IDocCommand
{
	public void Execute();
}

public class CopyCommand : IDocCommand
{

}

public class CutCommand : IDocCommand
{

}

public class PasteCommand : IDocCommand
{

}

public class UndoCommand : IDocCommand
{

}

public class RedoCommand : IDocCommand
{

}
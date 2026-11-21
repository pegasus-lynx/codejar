#load "l2_doc_interfaces.csx"

public interface IDocumentStorage
{
	public void Save(IDocument document);
}

public class LocalFileStorage : IDocumentStorage
{
	public LocalFileStorage(string path)
	{
		Path = path;
		FileStream fs = File.Create(path);
	}

	public string Path { get; init; }

	public void Save(IDocument document)
	{
		File.WriteAllText(Path, document.Render());
	}
}
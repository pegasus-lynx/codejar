#load "l2_doc_interfaces.csx"
#load "l2_doc_impl.csx"
#load "l3_doc_formatter.csx"
#load "l3_doc_storage.csx"

public class DocumentEditor
{
	public DocumentEditor(DocumentUser docUser, Document document = null)
	{
		if(document is null)
			document = New();
		
		_docUser = docUser;
		_document = new DocumentProxy(document, docUser);
		document.Subscribe((DocumentProxy)_document);

		_formatter = new DocFormatter(_document);
	}

	public static Document New()
	{
		Document document = new Document();
		return document;
	}

	public void Save(string file = null)
	{
		_storage = new LocalFileStorage(file);
		_storage.Save(_document);
	}

	public void Add(DocElement element)
	{
		if(_docUser.Role == UserRole.Editor)
			_document.Add(element);
	}

	public void RemoveAt(int index)
	{
		if(_docUser.Role == UserRole.Editor)
			_document.RemoveAt(index);
	}

	public void Insert(int index, DocElement element)
	{
		if(_docUser.Role == UserRole.Editor)
			_document.Insert(index, element);
	}

	public void View()
	{
	    Console.OutputEncoding = System.Text.Encoding.UTF8;

	    if(Console.IsOutputRedirected)
	        Console.Write("\u001b[2J\u001b[H");
	    else
	        Console.Clear();

		if(_formatter is not null)
			_formatter.Render(); 
			
		Console.WriteLine(_document.Render());
	}

	private DocumentProxy _document;
	private IDocumentStorage _storage;
	private DocFormatter _formatter;
	private DocumentUser _docUser;
}
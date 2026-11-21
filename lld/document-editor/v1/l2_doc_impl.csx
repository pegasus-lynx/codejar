#load "l0_enums.csx"
#load "l1_element.csx"
#load "l1_user.csx"
#load "l2_doc_interfaces.csx"

public class Document : IDocument, IDocObservable
{
	public Document() {}

	public void Add(DocumentEdit edit) => this.UpdateAndProcessQueue(edit);
	public void Insert(DocumentEdit edit) => this.UpdateAndProcessQueue(edit);
	public void RemoveAt(DocumentEdit edit) => this.UpdateAndProcessQueue(edit);

	public void Subscribe(IDocObserver observer) => _observers.Add(observer);

	public void Unsubscribe(IDocObserver observer) => _observers.Remove(observer);

	private void Notify(DocumentEdit edit)
	{
		foreach(IDocObserver observer in _observers)
			observer.Update(edit);
	}

	public List<DocElement> GetElements() => new List<DocElement>(_elements);

	public int GetVersion() => _version;

	public string Render()
	{
		StringBuilder sb = new();
		sb.AppendLine("<doc>");
		foreach(DocElement element in _elements)
			sb.AppendLine(element.Render());
		sb.AppendLine("</doc>");
		return sb.ToString();
	}

	private void UpdateAndProcessQueue(DocumentEdit edit)
	{
		_editQueue.Enqueue(edit);
		ProcessEditQueue();
	}

	private void ProcessEditQueue()
	{
		int count = _editQueue.Count;
		if(count == 0)
			return;

		List<DocumentEdit> _edits = new();
		while(count > 0)
		{
			_edits.Add(_editQueue.Dequeue());
			count -= 1;
		}

		_edits = TransformEdits(_edits);
		ApplyEdits(_edits);
	}

	private List<DocumentEdit> TransformEdits(List<DocumentEdit> _edits)
	{
		_version++;

		List<DocumentEdit> transformed = new();
		SortedList<int, int> shifts = new();

		foreach(DocumentEdit edit in _edits)
		{
			int shift = 0;
			foreach(int key in shifts.Keys)
			{
				if(edit.Index >= key)
					shift += shifts[key];
			}

			transformed.Add(new DocumentEdit(edit.Operation, edit.Index + shift,
											 edit.Element, edit.DocUser, _version));

			int operationShift = edit.Operation == DocumentEditOperation.Add ? 1 : -1;
			if(shifts.ContainsKey(edit.Index))
				shifts[edit.Index] += operationShift;
			else
				shifts.Add(edit.Index, operationShift);
		}

		return transformed;
	}

	private void ApplyEdits(List<DocumentEdit> edits)
	{
		foreach(DocumentEdit edit in edits)
		{
			if(edit.Operation == DocumentEditOperation.Add)
				_elements.Insert(edit.Index, edit.Element);
			else
				_elements.RemoveAt(edit.Index);

			Notify(edit);
		}
	}

	private int _version = 0;
	private List<DocElement> _elements = new();
	private List<IDocObserver> _observers = new();
	private Queue<DocumentEdit> _editQueue = new();
}

public class DocumentProxy : IDocument, IDocObserver
{
	public DocumentProxy(Document document,  DocumentUser docUser)
	{
		_docUser = docUser;
		_document = document;
		_version = document.GetVersion();
		_elements = document.GetElements();
	}

	public void Add(DocElement element)
	{
		if(_docUser.Role != UserRole.Editor)
			return;

		DocumentEdit edit = new DocumentEdit(DocumentEditOperation.Add,
											 _elements.Count, element,
											 _docUser, _version);
		Add(edit);
	}

	public void Insert(int index, DocElement element)
	{
		if(_docUser.Role != UserRole.Editor)
			return;

		DocumentEdit edit = new DocumentEdit(DocumentEditOperation.Add,
											 index, element,
											 _docUser, _version);
		Insert(edit);		
	}

	public void RemoveAt(int index)
	{
		if(_docUser.Role != UserRole.Editor)
			return;

		DocumentEdit edit = new DocumentEdit(DocumentEditOperation.Remove,
											 index, _elements[index],
											 _docUser, _version);
		RemoveAt(edit);				
	}

	public void Add(DocumentEdit edit) => _document.Add(edit);
	public void Insert(DocumentEdit edit) => _document.Insert(edit);
	public void RemoveAt(DocumentEdit edit) => _document.RemoveAt(edit);

	public string Render() => _document.Render();

	public void Update(DocumentEdit edit)
	{
		if(edit.Operation == DocumentEditOperation.Add)
		{
			if(edit.Index < _elements.Count)
			{
				_elements.Insert(edit.Index, edit.Element);
			}
			else
			{
				_elements.Add(edit.Element);
			}
		}
		else if(edit.Operation == DocumentEditOperation.Remove)
		{
			_elements.RemoveAt(edit.Index);
		}

		_version = edit.Version;
	}

	private DocumentUser _docUser;
	private int _version = 0;
	private IDocument _document;
	private List<DocElement> _elements;
}





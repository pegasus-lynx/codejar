#load "l0_enums.csx"
#load "l1_element.csx"

public interface IDocObserver
{
	public void Update(DocumentEdit operation);
}

public interface IDocObservable
{
	public void Subscribe(IDocObserver observer);
	public void Unsubscribe(IDocObserver observer);
	private void Notify(DocumentEdit edit);
}

public interface IDocument
{
	public void Add(DocumentEdit edit);
	public void Insert(DocumentEdit edit);
	public void RemoveAt(DocumentEdit edit);

	public string Render();
}

public record class DocumentEdit(DocumentEditOperation Operation, 
								 int Index, 
								 DocElement Element, 
								 DocumentUser DocUser,
								 int Version);
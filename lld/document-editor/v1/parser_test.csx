#load "l0_enums.csx"
#load "l1_element.csx"
#load "l1_user.csx"
#load "l2_doc_interfaces.csx"
#load "l3_doc_formatter.csx"

class TestDocument : IDocument
{
    public void Add(DocumentEdit edit) { }
    public void Insert(DocumentEdit edit) { }
    public void RemoveAt(DocumentEdit edit) { }
    public string Render() => "<doc>\n<text>Hi</text>\n</doc>";
}

DocParser parser = new(new TestDocument());

List<string> tokens = new();
while(!parser.EndOfFile())
    tokens.Add(parser.ReadNext());

if(tokens.Count == 0)
    throw new Exception("Parser returned no tokens");

Console.WriteLine(string.Join("|", tokens));

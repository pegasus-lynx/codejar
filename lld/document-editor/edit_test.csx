#load "l0_enums.csx"
#load "l1_element.csx"
#load "l1_user.csx"
#load "l2_doc_interfaces.csx"
#load "l2_doc_impl.csx"

DocumentUser editorUser = new(
    new User("Test Editor"),
    UserRole.Editor);

Document document = new(editorUser);
DocumentEdit edit = new(
    DocumentEditOperation.Add,
    0,
    new TextElement("Hello"),
    editorUser,
    0);

document.Add(edit);

if(!document.Render().Contains("<text>Hello</text>"))
    throw new Exception("First document edit was not applied");

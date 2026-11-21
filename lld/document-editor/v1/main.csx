#load "l3_doc_editor.csx"

DocumentUser editorUser = new(
    new User("Demo User"),
    UserRole.Editor);

DocumentEditor editor = new(editorUser);

ParagraphElement paragraph = new();
paragraph.Add(new TextElement("Document editors support text, images, formatting,"));
paragraph.Add(new TextElement("links, tables, and collaborative editing."));
paragraph.Add(new BoldDecorator
{
    Content = new TextElement("This bold demonstration exercises the text formatter.")
});

editor.Add(paragraph);
editor.Insert(0, new TextElement(
    "Welcome to the document editor. This sample contains enough text to demonstrate line wrapping and page formatting."));
editor.Add(new ParagraphElement());
editor.Insert(2, new TextElement(
    "The formatter reads document elements, measures each line, prints text, and separates pages when needed."));
editor.Add(new ImageElement("cover.png", 24, 12));

editor.View();
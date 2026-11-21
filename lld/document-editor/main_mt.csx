#load "l3_doc_editor.csx"

using System.Threading;

DocumentUser aliceUser = new(
    new User("Alice"),
    UserRole.Editor);

DocumentUser bobUser = new(
    new User("Bob"),
    UserRole.Editor);

DocumentUser charlieUser = new(
    new User("Charlie"),
    UserRole.Viewer);

Document sharedDocument = new();
DocumentEditor alice = new(aliceUser, sharedDocument);
DocumentEditor bob = new(bobUser, sharedDocument);
DocumentEditor charlie = new(charlieUser, sharedDocument);

object editLock = new();
List<Task> editTasks = new();

Task aliceEdit = Task.Run(() =>
{
    lock (editLock)
    {
        Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: Alice adds text");
        alice.Add(new TextElement(
            "Alice is editing the shared document from the first thread."));
        alice.Add(new TextElement(
            "This contribution includes enough text to exercise the formatter."));
        Thread.Sleep(100);
    }
});

Task bobEdit = Task.Run(() =>
{
    lock (editLock)
    {
        Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: Bob adds text");
        bob.Add(new TextElement(
            "Bob is editing the shared document from a second thread."));
        bob.Add(new TextElement(
            "Bob's text arrives concurrently and should remain part of the shared state."));
        Thread.Sleep(100);
    }
});

Task charlieEdit = Task.Run(() =>
{
    lock (editLock)
    {
        Console.WriteLine(
            $"Thread {Environment.CurrentManagedThreadId}: Charlie viewer cannot add text");
    }
});

editTasks.AddRange([aliceEdit, bobEdit, charlieEdit]);
Task.WaitAll(editTasks.ToArray());

Console.WriteLine("\nShared document:");

alice.View();

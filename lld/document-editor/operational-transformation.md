# Operational Transformation (OT) for Collaborative Editing

These notes explain how collaborative editors such as Google Docs model a document and how
Operational Transformation (OT) keeps every user's copy in sync. They also list what this
project's code needs in order to follow the same approach.

## 1. How the document is modelled

### Markup is a storage format, not the editing model

Real editors use markup (XML, HTML) only at the edges: files on disk, the clipboard, and export.
None of them edit a markup tree directly or re-parse markup to draw the page.

| Editor | In-memory model | Where markup appears |
|---|---|---|
| Google Docs | Structural elements (paragraphs, tables) containing **text runs** with a `textStyle`. Positions are flat character indexes (UTF-16 code units). | Export only. The page is drawn to a canvas from its own layout. |
| Quill | A **Delta**: a list of `insert` operations with attributes. A newline carries the paragraph attributes. | Import and export. |
| ProseMirror / Tiptap | A typed node tree with **marks** (bold, link) on text. Every position is still a single integer offset. | Import and export. |
| Microsoft Word | A piece table plus formatting runs. | `.docx` is XML on disk. |
| Yjs / Automerge (CRDTs) | A text sequence with formatting attributes on ranges. | Not used. |

Two consequences:

1. **Formatting belongs on ranges, not nested tags.** Bold over "lo Wo" in "Hello World" is one
   attribute on a range. With nested tags, the elements have to be split.
2. **Collaboration needs flat positions.** OT and CRDTs transform integer offsets. An index into a
   list of whole paragraphs cannot express two users typing in the same paragraph.

### Characters are addressed by index but stored as runs

**Logical view (what edits use):** the document is one flat sequence, and every character has an
integer index.

```
H e l l o   W o r l d \n
0 1 2 3 4 5 6 7 8 9 10 11
```

**Storage (what is kept in memory):** consecutive characters with the same formatting are grouped
into a run.

```
Run("Hello ", {})          // indexes 0-5
Run("World",  {bold})      // indexes 6-10
Run("\n",     {para: H1})  // index 11; the paragraph end carries the paragraph style
```

Edits use indexes and are applied to runs:

- `insertText(6, "big ")` inserts into the run at index 6, splitting or extending it.
- `updateTextStyle(0, 5, italic)` splits runs at the range boundaries and sets attributes.
- An image is an embedded item that takes one index.

**Persistence:** Google has said publicly that Docs saves a document as its history of changes.
Opening a document rebuilds it from that history, with snapshots for speed. This history also
powers version history. The exact internal formats are not public.

**CRDT editors differ slightly:** every character conceptually has a unique ID instead of an index.
This lets concurrent inserts merge without a central server. Internally, consecutive characters are
still merged into blocks for efficiency.

### A minimal model for this project

```csharp
public record TextAttributes(bool Bold = false, bool Underline = false, string? Link = null);

// An embed (for example, an image) counts as length 1.
public record Run(string Text, TextAttributes Attributes);

public abstract record Edit(int Version);
public record InsertEdit(int Position, string Text, TextAttributes Attrs, int Version) : Edit(Version);
public record DeleteEdit(int Position, int Length, int Version) : Edit(Version);
public record FormatEdit(int Position, int Length, TextAttributes Attrs, int Version) : Edit(Version);
```

The `DocElement` tree can still exist as a view or layout tree built from runs. It should not be
what users edit or what OT transforms.

## 2. How OT works

OT lets each user apply their own edit at once. The server then puts all edits into a single order.
Any edit made against an older version is **transformed** (its positions adjusted) against the
edits it missed.

### Server setup

- The server keeps the document, its `version`, and a `history` of applied edits.
- Each client sends `edit + baseVersion`, where `baseVersion` is the version the client was looking at.
- The server transforms the edit against `history[baseVersion..]`, applies it, increments the
  version, and broadcasts it.

### Example 1: two inserts

The document at **v5** is `Hello World`.

```
H e l l o _ W o r l d
0 1 2 3 4 5 6 7 8 9 10
```

| | Alice | Bob |
|---|---|---|
| Local edit | `Insert(6, "big ")` | `Insert(11, "!")` |
| Local view | `Hello big World` | `Hello World!` |
| Sent with | base v5 | base v5 |

On the server, Alice's edit arrives first:

1. Alice's edit has base v5, which is current, so it is applied unchanged. The document is now
   `Hello big World` at **v6**.
2. Bob's edit also has base v5, but v6 exists, so it is transformed against Alice's edit. Alice
   inserted 4 characters at position 6, and 6 ≤ 11, so Bob's position shifts by 4:
   `Insert(11, "!")` becomes `Insert(15, "!")`.
3. The server applies it. The document is now `Hello big World!` at **v7**.

On the clients:

- Alice receives `Insert(15, "!")`, applies it, and has `Hello big World!`.
- Bob receives Alice's `Insert(6, "big ")`. He first transforms it against his own unconfirmed
  `Insert(11)`. Since 6 < 11, it is unchanged. He applies it and has `Hello big World!`.

Both clients end with the same text, and the server and clients run the same transform rules.

### Example 2: delete versus format

The document at **v5** is `Hello World`.

- Alice: `Delete(0, 6)` removes `"Hello "`.
- Bob: `Format(6, 5, bold)` bolds `"World"`.

The server applies Alice's delete first, so the document becomes `World`. Bob's range starts at 6,
after the deleted range `[0, 6)`, so it shifts left by 6 and becomes `Format(0, 5, bold)`. The
result is **World** in bold.

If Bob's range had overlapped the deleted text, the range would shrink to the part that still exists.

### Transform rules

`T(a, b)` means "rewrite `a` so it applies after `b`".

| `a` \ `b` | `Insert(p, len)` | `Delete(p, n)` |
|---|---|---|
| **Insert at q** | If `p < q`, or `p == q` and `b` wins the tie, then `q += len`. | If `q ≥ p + n`, then `q -= n`. If `q` is inside the deleted range, then `q = p`. |
| **Delete at [q, q+m)** | If `p ≤ q`, then `q += len`. If `p` is inside the range, split into two deletes around the insert. | Remove the overlap: shift left by the deleted part before `q`, and shrink `m` by the overlapping part. |
| **Format at [q, q+m)** | Same as Delete. | Same as Delete. |

The tie rule matters. When two users insert at the same position, every site must choose the same
order, for example by lower user ID or by server order.

### Server code sketch

```csharp
public int Submit(Edit edit)
{
    lock (_lock)
    {
        foreach (Edit applied in _history.Skip(edit.Version))
            edit = Transform(edit, applied);

        Apply(edit);
        _history.Add(edit);
        Broadcast(edit with { Version = _history.Count });
        return _history.Count;
    }
}

static Edit Transform(Edit a, Edit b) => (a, b) switch
{
    (InsertEdit x, InsertEdit y) when y.Position <= x.Position
        => x with { Position = x.Position + y.Text.Length },
    (InsertEdit x, DeleteEdit y) when y.Position < x.Position
        => x with { Position = Math.Max(y.Position, x.Position - y.Length) },
    // The delete/delete, delete/insert, and format cases follow the table above.
    _ => a
};
```

## 3. Multiple edits from each user

The server still transforms one edit at a time. The trap is the **context** of each edit: a user's
second edit assumes their first edit already happened, but the server may not have applied it yet.

### Example

The starting document is `abc` at **v0**.

```
a b c
0 1 2
```

| | Alice | Bob |
|---|---|---|
| Edit 1 | A1 `Insert(0, "X")` gives `Xabc` | B1 `Insert(1, "P")` gives `aPbc` |
| Edit 2 | A2 `Insert(4, "Y")` gives `XabcY` | B2 `Insert(3, "Q")` gives `aPbQc` |

Bob intends `Q` to go between `b` and `c`. The server applies A1 and A2 first, so the document is
`XabcY` at **v2**.

### Wrong: transforming each of Bob's edits against A1 and A2

- B1 `Insert(1)`: A1 is before it, so it becomes 2. A2 at 4 is after it, so it stays 2.
  The result is `XaPbcY`, which is correct.
- B2 `Insert(3)`: A1 makes it 4. A2 at 4 is ≤ 4, so it becomes **5**.
  The result is `XaPbcQY`, which is **wrong**.

The cause: A2's position 4 was measured on `Xabc`, a document without `P`. B2's position was
measured on `aPbc`, a document with `P`. Comparing positions from different documents is meaningless.

### Right: transform both sides step by step

When B1 moves past Alice's edits, Alice's edits must also move past B1. This gives A1′ and A2′,
which describe Alice's edits on a document that already contains `P`.

```
           A1            A2
v0 ─────────●─────────────●──────── server
   \        │             │
 B1 \   T(A1,B1)=A1′   T(A2,B1′)=A2′
     \      │             │
      ●─────●─────────────●  B1 moves to B1″ = Insert(2, "P")
     /      │             │
 B2 /   T(B2,A1′)     T(B2′,A2′)
```

| Step | Result |
|---|---|
| `T(A1, B1)`: P is at 1, after 0 | A1′ = `Insert(0, "X")`; B1′ = `Insert(2, "P")` |
| `T(A2, B1′)`: P is at 2, before 4 | A2′ = `Insert(5, "Y")`; B1″ = `Insert(2, "P")` |
| `T(B2, A1′)`: X is at 0, before 3 | `Insert(4, "Q")` |
| `T(B2′, A2′)`: Y is at 5, after 4 | `Insert(4, "Q")` |

Applying B1″ and then B2 to `XabcY` gives `XaPbQcY`, which is correct.

Alice receives `Insert(2, "P")` and then `Insert(4, "Q")`, and also ends with `XaPbQcY`.
Both copies converge.

### How real systems avoid the grid: one edit in flight

Google's protocols (Jupiter, Wave, and Docs) and ot.js make clients send **one edit at a time and
wait for an acknowledgement**. Everything typed meanwhile goes into a local buffer.

The client has three states:

```
Synchronized ──local edit──▶ AwaitingAck(inflight)
AwaitingAck ──local edit──▶ AwaitingAckWithBuffer(inflight, buffer)
On ack: send the buffer as the new inflight edit, or return to Synchronized.
```

When a server edit `s` arrives, the client runs the ladder from above locally:

```csharp
void OnServerEdit(Edit s)
{
    (inflight, s) = TransformPair(inflight, s);   // Both sides move.
    if (buffer != null)
        (buffer, s) = TransformPair(buffer, s);
    Apply(s);                                     // s now fits the local document.
}
```

Because the client sends `buffer` only after `inflight` is acknowledged, every edit that reaches
the server is based on a version the server already has. The server then needs only the single
history loop shown in the server code sketch.

### The buffer requires combining edits

Many keystrokes must merge into one buffered edit. Real systems therefore represent an edit as an
operation that covers the whole document, built from `retain(n)`, `insert("...")`, and `delete(n)`.
Quill calls these Deltas, and ot.js calls them `TextOperation`. They support two operations:

- `Compose(a, b)`: combine two consecutive edits into one.
- `Transform(a, b)`: produce `(a′, b′)` such that applying `a` then `b′` equals applying `b` then `a′`.

## 4. What this project's code needs

- **`Document`:** keep a `_history` of applied edits. Transform each incoming edit against
  `_history.Skip(edit.Version)` before applying it, and guard the whole step with a lock.
- **`DocumentProxy`:** add `inflight` and `buffer` fields and the `OnServerEdit` logic, so incoming
  server edits are transformed against the client's own unconfirmed edits.
- **`TransformEdits`:** remove the "transform within a batch" approach. The batch always holds one
  edit, and real collaborative editors do not transform a batch of concurrent edits against each
  other.
- **`DocumentEdit.Index`:** change it from an index into the top-level element list to a character
  position, so edits inside a paragraph can be expressed and transformed.

# Document Editor

There are different types of document editors. 

- A document editor can be like - Notepad, Sublime, etc. 
  - These are line based editors. A line can be infinitely long.
  - Other are MS Word / Google Docs - where the view is based on page width. These support different types of media, etc.

So it is important to know which type of viewer you are writing. We will refer to the first one as text / code editors, wherease second one as document editor.

## Functional Requirements

**Document Editor Features**
1. Ability to add/remove text, media (images, charts, tables, link) etc.
2. Ability to undo / redo recent edits.
3. Provide support for cursor movements.
4. Ability to open / save new documents.

**Collaboration Features**
1. One or more users can edit the document at the same time.
2. All clients converge to the same state.
3. Offline support - when system is offline the app supports normal features.
4. Latency - local edits are instantaneous, remote edits arrive with slight delay
5. Viewers - who can see the real-time edits.

### Non-Functional Requirements

1. Easy addition of new features - formatting, etc.
2. Scalable design, fast edits

### Class Design - v1

#### Entities

- **Document**: The actual server managed document instance.
- **DocumentProxy**: The proxy object on the client machine that manages the sync, networking, etc.
- **abs. DocElement**: The base object, the document is a collection of this.
- **Text, Image, etc.**: Concrete implementation of DocElements.
- **Common Commands**: Undo/Redo, Cut/Copy/Paste
- **DocUser : DocObserver**: The user either making the edits of 
- **DocFormatter** : Formats the document for printing. Possibly creates the view.
- **DocView** : What the clients see, non-business things.
- **Cursor** ??

#### Task Items
- [x] Add Document Elements
- [x] Add Document User classes
- [x] Add Document and proxy class
- [x] Add Document Editor class
- [] Add Document command system
- [] Add observation capability
- [x] Add document formatting option
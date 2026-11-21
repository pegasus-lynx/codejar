public abstract class DocElement
{
	public abstract string Render();	
}

public class CompositeElement : DocElement
{
	public List<DocElement> Elements { get; init; } = new();
	
	public override string Render()
	{
		string content = "";
		foreach(DocElement element in Elements)
			content += element.Render();
		return content;
	}

	public void Add(DocElement element) => Elements.Add(element);
	public void Remove(DocElement element) => Elements.Remove(element);
	public void Insert(int index, DocElement element) => Elements.Insert(index, element);
	public void Remove(int index) => Elements.RemoveAt(index);
}

public class NewLineElement : DocElement
{
	public override string Render() => "<linebreak/>";
}

public class TabElement : DocElement
{
	public override string Render() => "<tab/>";
}

public class TextElement : DocElement
{
	public TextElement(string text) { _text = text; }

	public override string Render() => $"<text>{_text}</text>";

	private string _text = "";
}

public class ParagraphElement : CompositeElement
{
	public override string Render()
	{
		string content = String.Join(" ", Elements.Select(el => el.Render()).ToList());
		return "<para>" + content + "</para>";
	}
}

public class ImageElement : DocElement
{
	public ImageElement(string imagePath, int height, int width)
	{
		ImagePath = imagePath;
		SetSize(height, width);
	}

	private void SetSize(int height, int width)
	{
		if(height <= 0 || width <= 0)
			throw new ArgumentException("Invalid image sie");

		Width = width;
		Height = height;
	}

	public string ImagePath { get; init; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	public override string Render()
	{
		string content = $"<image height='{Height}' width='{Width}' path='{ImagePath}'/>"; 
		content += (new NewLineElement()).Render();
		return content;
	}
}

// Decorators

public abstract class DecoratorElement : DocElement
{
	public abstract string DecoratorTag { get; init; }
	public DocElement Content { get; init; }

	public override string Render()
	{
		return $"<{DecoratorTag}>" + Content.Render() + $"</{DecoratorTag}>";
	}
}

public class HyperlinkDecorator : DecoratorElement
{
	public override string DecoratorTag { get; init; } = "link";
}

public class UnderlineDecorator : DecoratorElement
{
	public override string DecoratorTag { get; init; } = "underline";
}

public class BoldDecorator : DecoratorElement
{
	public override string DecoratorTag { get; init; } = "bold";
}


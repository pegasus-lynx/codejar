#load "l0_enums.csx"
#load "l2_doc_interfaces.csx"

using System.Text.RegularExpressions;

public class DocFormatter
{
	public DocFormatter(IDocument document)
	{
		_document = document;
		SetPageSize(30, 30);
		SetPageAlignment(Alignment.Left);
	}

	public void SetPageSize(int width, int height)
	{
		if(width <= 0 || height <= 0)
			throw new ArgumentException($"Invalid page dimensions {height} x {width}");

		_width = width;
		_height = height;
	}

	public void SetPageAlignment(Alignment alignment) => _alignment = alignment;

	public void Render()
	{
		DocParser parser = new DocParser(_document);
		while(!parser.EndOfFile())
		{
			string token = parser.ReadNext();

			if(!DocNode.IsTag(token))
			{
				char[] delims = {' ', '\n', '\r'};
				string[] parts = token.Split(delims);
				foreach(string part in parts)
				{					
					if(part.Length + currentLinePos > _width)
						PrintEndLine();

					Console.Write($"{part} ");
					currentLinePos += part.Length + 1;
				}
			}
			else if(DocNode.IsImageTag(token))
			{
				PrintImage(token);
			}
			else if(DocNode.IsEmptyTag(token))
			{
				if(token == "<linebreak/>")
					PrintEndLine();

				if(token == "<tab/>")
				{
					if(_tabWidth + currentLinePos >= _width)
						PrintEndLine();
					else
						Console.Write("\t");					
				}
			}
			else if(DocNode.IsStartTag(token))
			{
				Console.Write(resetFormatString);

				if(token.Contains("link"))
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					formatString.Add("\x1B[4m");
				}
				else if(token.Contains("bold"))
				{
					formatString.Add("\x1B[4m");
				}
				else if(token.Contains("underline"))
				{
					formatString.Add("\x1B[4m");
				}

				Console.Write(string.Join("", formatString));
			}
			else
			{
				if(token.Contains("para"))
				{
					IncrementLine();
					IncrementLine();
					continue;
				}

				Console.Write(resetFormatString);

				if(token.Contains("link"))
				{
					Console.ForegroundColor = ConsoleColor.White;
					formatString.Remove("\x1B[4m");
				}
				else if(token.Contains("bold"))
				{
					formatString.Remove("\x1B[1m");
				}
				else if(token.Contains("underline"))
				{
					formatString.Remove("\x1B[4m");
				}
				
				Console.Write(string.Join("", formatString));
			}
		}
	}

	private void PrintImage(string token)
	{
		IncrementLine();
		Console.WriteLine("######################");
		Console.WriteLine("#                    #");
		Console.WriteLine("#       Image        #");
		Console.WriteLine("#                    #");
		Console.WriteLine("######################");
		IncrementLine();
	}

	private void PrintEndLine()
	{
		Console.WriteLine();
		IncrementLine();
		currentLinePos = 0;
	}

	private void PrintPageMargin()
	{
		Console.WriteLine();
		Console.WriteLine();
	}

	private void PrintPageSeparator()
	{
		string sep = "";
		for(int i=0;i<_width;i++)
			sep += "_";
		Console.WriteLine(sep);
	}

	private void IncrementLine()
	{
		currentLine++;
		if(currentLine == _height)
		{
			currentLine = 0;
			PrintPageMargin();
			PrintPageSeparator();
			PrintPageMargin();
		}
	}

	private List<string> formatString = new();
	private string resetFormatString = "\x1B[0m";

	private int currentLinePos = 0;
	private int currentLine = 0;

	private int _width;
	private int _height;
	private int _tabWidth = 4;
	private IDocument _document;
	private Alignment _alignment;
}


public class DocParser
{
	public DocParser(IDocument document)
	{
		_docString = document.Render();
	}

	public string ReadNext()
	{
		int pos = FindNextTag(out string tag);
		if(pos == _position && pos < _docString.Length)
		{
			string token = tag;
			_position += tag.Length;
			return token;
		}

		if(pos > _position)
		{
			string token = _docString.Substring(_position, pos - _position);
			_position = pos;
			return token;
		}

		_position = _docString.Length;
		return "";
	}

	private int FindNextTag(out string tag)
	{
		tag = "";

		if(_position >= _docString.Length)
			return _docString.Length;

		int tagStart = _docString.IndexOf('<', _position);
		if(tagStart == -1)
			return _docString.Length;

		if(tagStart > _position)
			return tagStart;

		int tagEnd = _docString.IndexOf('>', tagStart);
		if(tagEnd == -1)
		{
			tag = _docString.Substring(tagStart);
			return tagStart;
		}

		tag = _docString.Substring(tagStart, tagEnd - tagStart + 1);
		return tagStart;
	}

	public bool EndOfFile() => _position == _docString.Length;

	private int _position = 0;
	private string _docString;
}


public static class DocNode
{
	public static bool IsTag(string token)
	{
		return IsStartTag(token) || IsEndTag(token) || IsImageTag(token);
	}

	public static bool IsImageTag(string token) => imageTag.IsMatch(token);
	public static bool IsEmptyTag(string token) => emptyTag.IsMatch(token);
	public static bool IsStartTag(string token) => startTag.IsMatch(token);
	public static bool IsEndTag(string token) => endTag.IsMatch(token);

	private static Regex emptyTag = new Regex(@"<(\S+)/>");
	private static Regex imageTag = new Regex(@"<image(.?)/>");
	private static Regex startTag = new Regex(@"<.+>");
	private static Regex endTag = new Regex(@"</.+>");
}
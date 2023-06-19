namespace InfoSearch.Core.Model;

public class Document
{
    public string Name { get; init; }

    public string Text { get; init; }

    public Document(string name, string text)
    {
        Name = name;
        Text = text;
    }

    public Document() { }
}

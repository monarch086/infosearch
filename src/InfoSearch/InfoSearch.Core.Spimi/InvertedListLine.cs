namespace InfoSearch.Core.Spimi;

internal class InvertedListLine
{
    public string Key { get; set; } = string.Empty;
    public IList<int> DocIds { get; set; } = new List<int>();
}
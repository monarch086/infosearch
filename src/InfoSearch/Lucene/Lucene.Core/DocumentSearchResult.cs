using InfoSearch.Core.Model;

namespace Lucene.Core;

public class DocumentSearchResult : Document
{
    public float Score { get; init; }

    public override string ToString()
    {
        return $"Score {Score:f4}: {Name}";
    }
}

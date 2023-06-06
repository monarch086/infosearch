using InfoSearch.Core;
using InfoSearch.Core.Indexes;

namespace InfoSearch.QueryProcessing.Model;

public class TwoWordQuery : IQuery<WordPair>
{
    public IList<WordPair> Components { get; set; }
}

namespace InfoSearch.QueryProcessing.Model;

public class CoordinateQueryComponent
{
    public int Distance { get; set; } = 1;

    public string Term { get; set; }

    public override string ToString()
    {
        return $"term \'{Term}\' in distance {Distance} from previous term";
    }
}

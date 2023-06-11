namespace InfoSearch.PgSearch.Model;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; }

    public Author Author { get; set; }

    public override string ToString()
    {
        return $"{Title} by {Author?.FirstName} " +
            $"{(string.IsNullOrEmpty(Author?.MiddleName) ? string.Empty : Author?.MiddleName + " ")}" +
            $"{Author?.LastName}";
    }
}
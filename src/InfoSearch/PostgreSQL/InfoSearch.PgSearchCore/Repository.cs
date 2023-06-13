using Dapper;
using Dapper.FluentMap;
using InfoSearch.PgSearch.Model;
using InfoSearch.PgSearchCore.Mappers;
using Npgsql;

namespace InfoSearch.PgSearchCore;

public class Repository
{
    private const string USER = "postgres";
    private const string PASSWORD = "postgres";
    private const string HOST = "localhost"; //"192.168.0.185";
    private const string PORT = "5432";
    private const string DATABASE = "extensions";

    private string ConnectionString => $"User ID={USER};Password={PASSWORD};Host={HOST};Port={PORT};Database={DATABASE};";

    public Repository()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new AuthorMap());
        });
    }

    public IEnumerable<Book> SearchByAuthor(string name)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            var sql =
            @"SELECT * FROM books b
            INNER JOIN authors a ON a.id = b.author_id
            WHERE soundex(a.first_name) = soundex(@searchName)
                OR soundex(a.middle_name) = soundex(@searchName)
                OR soundex(a.last_name) = soundex(@searchName)
            ORDER BY b.title";

            var books = connection.Query<Book, Author, Book>(
                sql,
                (book, author) => { book.Author = author; return book; },
                new { searchName = name });

            return books;
        }
    }

    public IEnumerable<Book> SearchByTitle(string title)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            var sql =
            @"SELECT * FROM books b
            INNER JOIN authors a ON a.id = b.author_id
            WHERE title % @title
            ORDER BY b.title";

            var books = connection.Query<Book, Author, Book>(
                sql,
                (book, author) => { book.Author = author; return book; },
                new { title });

            return books;
        }
    }
}
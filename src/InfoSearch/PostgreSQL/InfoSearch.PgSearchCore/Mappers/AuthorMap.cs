using Dapper.FluentMap.Mapping;
using InfoSearch.PgSearch.Model;

namespace InfoSearch.PgSearchCore.Mappers
{
    internal class AuthorMap : EntityMap<Author>
    {
        internal AuthorMap()
        {
            Map(a => a.FirstName).ToColumn("first_name");
            Map(a => a.MiddleName).ToColumn("middle_name");
            Map(a => a.LastName).ToColumn("last_name");
        }
    }
}

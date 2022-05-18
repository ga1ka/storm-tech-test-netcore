using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.Tests.Builders
{
    public class InMemoryApplicationDbContextBuilder
    {
        public ApplicationDbContext Build()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new ApplicationDbContext(contextOptions);

            dbContext.Database.Migrate();

            return dbContext;
        }
    }
}

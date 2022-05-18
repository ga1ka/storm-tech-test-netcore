using FluentAssertions;
using System.Linq;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.Services.Data
{
    public sealed class WhenSingleTodoListIsLoaded
    {
        private const int todoListId = 4242;
        private readonly ApplicationDbContext dbContext;

        public WhenSingleTodoListIsLoaded()
        {
            var user = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "42", UserName = "arthur.dent", Email = "arthur.dent@galaxy.com" };

            dbContext = new MockedApplicationDbContextBuilder()
                .WithList(new TodoList(user, "list 1")
                {
                    TodoListId = todoListId,
                    Items = new[] {
                    new TodoItem(todoListId, user, "i1", Importance.High) { IsDone = false },
                    new TodoItem(todoListId, user, "i2", Importance.Low) { IsDone = true },
                    new TodoItem(todoListId, user, "i3", Importance.Medium) { IsDone = false }
                }
                })
                .Build();

        }

        [Fact]
        public void WithoutNotDoneOnlyFlag()
        {
            var result = dbContext.SingleTodoList(todoListId);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Count.Should().Be(3);
            result.Items.Any(x => x.IsDone).Should().BeTrue();
        }

        [Fact(Skip = "Require more mocking for IIncludableQueryable<>")]
        public void WithNotDoneOnlyFlag()
        {
            var result = dbContext.SingleTodoList(todoListId, true);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Count.Should().Be(2);
            result.Items.Any(x => x.IsDone).Should().BeFalse();
        }
    }
}

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            dbContext = new InMemoryApplicationDbContextBuilder().Build();

            var user = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "42", UserName = "arthur.dent", Email = "arthur.dent@galaxy.com" };
            dbContext.Users.Add(user);


            dbContext.TodoLists.Add(new TodoList(user, "list 1") { TodoListId = todoListId,
                Items = new[] {
                    new TodoItem(todoListId, user, "i1", Importance.High) { IsDone = false },
                    new TodoItem(todoListId, user, "i2", Importance.Low) { IsDone = true },
                    new TodoItem(todoListId, user, "i3", Importance.Medium) { IsDone = false }
                }
            });

            dbContext.SaveChanges();
        }

        [Fact(Skip = "EF Include where seems to be not working with sqllite in-memory provider")]
        public void WithoutNotDoneOnlyFlag()
        {
            var result = dbContext.SingleTodoList(todoListId);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Count.Should().Be(3);
            result.Items.Any(x => x.IsDone).Should().BeTrue();
        }

        [Fact(Skip = "EF Include where seems to be not working with sqllite in-memory provider")]
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

using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.EntityModelMappers.TodoLists.Details
{
    public sealed class WhenItemsImportanceIsDifferent
    {
        private readonly IdentityUser srcIdentityUser = new("arthur.dent") { Email = "arthur.dent@galaxy.com" };
        private readonly TodoList srcTodoList;

        public WhenItemsImportanceIsDifferent()
        {
            srcTodoList = new TodoList(srcIdentityUser, "Personal");

            srcTodoList = new TestTodoListBuilder(srcIdentityUser, "Personal")
                .WithItem("m1", Importance.Medium)
                .WithItem("l1", Importance.Low)
                .WithItem("m2", Importance.Medium)
                .WithItem("h2", Importance.High)
                .WithItem("l2", Importance.Low)
                .Build();
        }

        [Fact]
        public void OrderIsCorrect()
        {
            var result = TodoListDetailViewmodelFactory.Create(srcTodoList);

            result.Should().NotBeNull();

            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(srcTodoList.Items.Count);

            result.Items.Select(x => x.Importance).Should().BeInAscendingOrder();
        }
    }
}

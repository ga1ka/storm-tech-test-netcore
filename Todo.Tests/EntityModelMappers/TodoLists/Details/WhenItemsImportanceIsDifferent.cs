using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
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

            srcTodoList.Items.Add(new TodoItem(2, srcIdentityUser, "m1", Importance.Medium));
            srcTodoList.Items.Add(new TodoItem(4, srcIdentityUser, "l1", Importance.Low));
            srcTodoList.Items.Add(new TodoItem(8, srcIdentityUser, "h1", Importance.High));
            srcTodoList.Items.Add(new TodoItem(42, srcIdentityUser, "m2", Importance.Medium));
            srcTodoList.Items.Add(new TodoItem(46, srcIdentityUser, "h2", Importance.High));
            srcTodoList.Items.Add(new TodoItem(44, srcIdentityUser, "l2", Importance.Low));
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

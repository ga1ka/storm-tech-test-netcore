using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.EntityModelMappers.TodoLists.Details
{
    public sealed class WhenOrderByRankIsUsed
    {
        private readonly IdentityUser srcIdentityUser = new("arthur.dent") { Email = "arthur.dent@galaxy.com" };
        private readonly TodoList srcTodoList;

        public WhenOrderByRankIsUsed()
        {
            srcTodoList = new TestTodoListBuilder(srcIdentityUser, "Personal")
                .WithItem("m1", Importance.Medium, 42)
                .WithItem("l1", Importance.Low, 22)
                .WithItem("h1", Importance.High, 8)
                .WithItem("m2", Importance.Medium, 41)
                .WithItem("h2", Importance.High, 20)
                .WithItem("l2", Importance.Low)
                .Build();
        }

        [Fact]
        public void OrderIsCorrect()
        {
            var result = TodoListDetailViewmodelFactory.Create(srcTodoList, orderByRankMode: true);

            result.Should().NotBeNull();

            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(srcTodoList.Items.Count);

            result.Items.Select(x => x.Rank).Should().BeInDescendingOrder();
        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.Services.Data
{
    public  class WhenRelevantTodoLists
    {
        private static readonly IdentityUser firstUser = new("arthur.dent") { Email = "arthur.dent@galaxy.com" };
        private static readonly IdentityUser secondUser = new("ford.perfect@galaxy.com");

        private static readonly TodoList listOwnedByFirstUser = new TestTodoListBuilder(firstUser, "Travel")
            .WithItem("Towel", Importance.High)
            .WithItem("Small Towel", Importance.Medium)
            .Build();
        private static readonly TodoList listOwnedBySecondUser = new TestTodoListBuilder(secondUser, "Shopping")
            .WithItem("New blue Towel", Importance.High)
            .WithItem("New yellow Towel", Importance.Medium)
            .Build();
        private static readonly TodoList listOwnedBySecondUserWithItemAssignedToFirstUser = new TestTodoListBuilder(secondUser, "Personal")
            .WithItem("Meet with Zaphod", Importance.High)
            .WithItem("Buy Milk", Importance.Medium)
            .WithItem("Meet with president", Importance.Low, firstUser.Id)
            .Build();

        private readonly ApplicationDbContext dbContext;

        public WhenRelevantTodoLists() => dbContext = new MockedApplicationDbContextBuilder()
                .WithLists(listOwnedByFirstUser, listOwnedBySecondUser, listOwnedBySecondUserWithItemAssignedToFirstUser)
                .Build();

        [Fact]
        public void ContainsAlsoListOwnedBySecondUserButWithItemAssingedToFirstUser()
        {
            var result = dbContext.RelevantTodoLists(firstUser.Id).ToList();

            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(listOwnedByFirstUser);
            result.Should().Contain(listOwnedBySecondUserWithItemAssignedToFirstUser);
        }

    }
}

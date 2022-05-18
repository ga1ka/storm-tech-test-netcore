using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Todo.Data;
using Todo.Data.Entities;


namespace Todo.Tests.Builders
{
    public class MockedApplicationDbContextBuilder
    {
        private readonly List<TodoList> allTodoList = new();

        public MockedApplicationDbContextBuilder WithList(TodoList list) => WithLists(list);

        public MockedApplicationDbContextBuilder WithLists(params TodoList[] lists)
        {
            allTodoList.AddRange(lists);
            return this;
        }

        public ApplicationDbContext Build()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.TodoLists).Returns(SetupDbSet(allTodoList));

            return mockContext.Object;
        }

        private static DbSet<T> SetupDbSet<T>(List<T> items) where T: class
        {
            var result = new Mock<DbSet<T>>();

            var queryableItems = items.AsQueryable();

            result.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableItems.Provider);
            result.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableItems.Expression);
            result.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableItems.ElementType);
            result.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableItems.GetEnumerator());

            return result.Object;
        }
    }
}

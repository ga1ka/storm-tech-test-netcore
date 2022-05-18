using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services
{
    public static class ApplicationDbContextConvenience
    {
        public static IQueryable<TodoList> RelevantTodoLists(this ApplicationDbContext dbContext, string userId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .Where(tl => tl.Owner.Id == userId);
        }

        public static TodoList SingleTodoList(this ApplicationDbContext dbContext, int todoListId, bool notDoneOnly = false)
        {
            IIncludableQueryable<TodoList, IEnumerable<TodoItem>> query = notDoneOnly
                ? dbContext.TodoLists.Include(tl => tl.Owner)
                               .Include(tl => tl.Items.Where(ti => !ti.IsDone))
                : dbContext.TodoLists.Include(tl => tl.Owner)
                               .Include(tl => tl.Items);

            return query.ThenInclude(ti => ti.ResponsibleParty)
                               .Single(tl => tl.TodoListId == todoListId);
        }

        public static TodoItem SingleTodoItem(this ApplicationDbContext dbContext, int todoItemId)
        {
            return dbContext.TodoItems.Include(ti => ti.TodoList).Single(ti => ti.TodoItemId == todoItemId);
        }
    }
}
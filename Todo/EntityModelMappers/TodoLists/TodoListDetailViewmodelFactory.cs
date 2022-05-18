using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool notDoneOnlyMode = false, bool orderByRankMode = false)
        {
            var items = todoList.Items.Select(TodoItemSummaryViewmodelFactory.Create);

            items = orderByRankMode ? items.OrderByDescending(x => x.Rank) : items.OrderBy(x => x.Importance);

            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items.ToArray(), notDoneOnlyMode, orderByRankMode);
        }
    }
}
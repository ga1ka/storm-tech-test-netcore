using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool notDoneOnlyMode = false)
        {
            var items = todoList.Items.Select(TodoItemSummaryViewmodelFactory.Create).OrderBy(x => x.Importance).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, notDoneOnlyMode);
        }
    }
}
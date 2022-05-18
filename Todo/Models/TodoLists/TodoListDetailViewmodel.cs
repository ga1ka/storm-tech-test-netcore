using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }

        public bool NotDoneOnlyMode { get; }

        public bool OrderByRankMode { get; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool notDoneOnlyMode, bool orderByRankMode)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            NotDoneOnlyMode = notDoneOnlyMode;
            OrderByRankMode = orderByRankMode;
        }
    }
}
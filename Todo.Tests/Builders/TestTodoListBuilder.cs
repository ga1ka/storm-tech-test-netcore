using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;

namespace Todo.Tests.Builders
{
    /*
     * This class makes it easier for tests to create new TodoLists with TodoItems correctly hooked up
     */
    public class TestTodoListBuilder
    {
        private readonly string title;
        private readonly IdentityUser owner;
        private readonly List<(string Titile, Importance Importance, IdentityUser ResponsibleParty)> items = new();

        public TestTodoListBuilder(IdentityUser owner, string title)
        {
            this.title = title;
            this.owner = owner;
        }

        public TestTodoListBuilder WithItem(string itemTitle, Importance importance) => WithItem(itemTitle, importance, owner);

        public TestTodoListBuilder WithItem(string itemTitle, Importance importance, string responsiblePartyId)
            => WithItem(itemTitle, importance, new IdentityUser { Id = responsiblePartyId });

        public TestTodoListBuilder WithItem(string itemTitle, Importance importance, IdentityUser responsibleParty)
        {
            items.Add((itemTitle, importance, responsibleParty));
            return this;
        }

        public TodoList Build()
        {
            var todoList = new TodoList(owner, title);
            var todoItems = items.Select(itm => new TodoItem(todoList.TodoListId, itm.ResponsibleParty, itm.Titile, itm.Importance));
            todoItems.ToList().ForEach(tlItm =>
            {
                todoList.Items.Add(tlItm);
                tlItm.TodoList = todoList;
            });
            return todoList;
        }
    }
}
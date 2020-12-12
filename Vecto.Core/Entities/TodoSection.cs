using System.Collections.Generic;
using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public class TodoSection : Section<TodoItem>
    {
        public string Name { get; set; }
        public IList<TodoItem> Items { get; } = new List<TodoItem>();

        public TodoSection() { }
    }
}
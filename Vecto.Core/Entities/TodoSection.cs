using System.Collections.Generic;

namespace Vecto.Core.Entities
{
    public class TodoSection : Section
    {
        public IList<TodoItem> Items { get; } = new List<TodoItem>();
    }
}
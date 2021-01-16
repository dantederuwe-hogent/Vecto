using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Application.Items
{
    public static class ItemMapper
    {
        public static void UpdateWith(this ISectionItem item, ItemDTO model)
        {
            switch (item)
            {
                case PackingItem packingItem:
                    packingItem.Title = model.Title ?? packingItem.Title;
                    packingItem.Description = model.Description ?? packingItem.Description;
                    packingItem.Amount = model.Amount;
                    return;
                case TodoItem todoItem:
                    todoItem.Title = model.Title ?? todoItem.Title;
                    todoItem.Description = model.Description ?? todoItem.Description;
                    return;
            }
        }

        public static PackingItem MapToPackingItem(this ItemDTO model) =>
            new PackingItem
            {
                Title = model.Title,
                Description = model.Description,
                Amount = model.Amount
            };

        public static TodoItem MapToTodoItem(this ItemDTO model) =>
            new TodoItem
            {
                Title = model.Title,
                Description = model.Description
            };
    }
}

using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public class PackingItem : EntityBase, ISectionItem, IToggleable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
        public int Amount { get; set; }

        public void Toggle() => Checked = !Checked;

        public PackingItem() { }
    }
}

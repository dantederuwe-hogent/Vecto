using System.Collections.Generic;
using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public class PackingSection : EntityBase, ISection<PackingItem>
    {
        public string Name { get; set; }
        public IList<PackingItem> Items { get; } = new List<PackingItem>();

        public PackingSection() { }
    }
}
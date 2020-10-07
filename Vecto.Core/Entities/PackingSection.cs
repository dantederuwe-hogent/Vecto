using System.Collections.Generic;

namespace Vecto.Core.Entities
{
    public class PackingSection : Section
    {
        public IList<PackingItem> Items { get; } = new List<PackingItem>();
    }
}
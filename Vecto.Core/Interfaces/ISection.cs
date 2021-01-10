using System.Collections.Generic;

namespace Vecto.Core.Interfaces
{
    public interface ISection<TItem> : ISection where TItem : ISectionItem
    {
        public IList<TItem> Items { get; }
    }

    public interface ISection
    {
        public string Name { get; set; }
    }
}

using System.Collections.Generic;

namespace Vecto.Core.Interfaces
{

    public interface ISection
    {
        public string Name { get; set; }
    }

    public interface ISection<T> : ISection where T : ISectionItem
    {
        public IList<T> Items { get; }
    }
}
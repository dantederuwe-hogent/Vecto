using System.Collections.Generic;
using Vecto.Core.Entities;

namespace Vecto.Core.Interfaces
{

    public abstract class Section : EntityBase
    {
        public string Name { get; set; }
    }

    public abstract class Section<T> : Section where T : ISectionItem
    {
        public IList<T> Items { get; }
    }
}
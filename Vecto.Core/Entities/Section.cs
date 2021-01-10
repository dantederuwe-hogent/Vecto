using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public abstract class Section : EntityBase, ISection
    {
        public string Name { get; set; }

        public string SectionType { get; set; }
    }
}

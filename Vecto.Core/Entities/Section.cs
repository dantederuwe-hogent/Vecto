using Vecto.Core.Interfaces;

namespace Vecto.Core.Entities
{
    public class Section : EntityBase, ISection
    {
        public string Name { get; set; }

        public string SectionType { get; set; }
    }
}

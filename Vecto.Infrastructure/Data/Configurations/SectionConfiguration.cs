using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vecto.Core.Entities;

namespace Vecto.Infrastructure.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            var discriminatorBuilder = builder.HasDiscriminator<string>(nameof(Section.SectionType));
            var subtypes = typeof(Section).Assembly.DefinedTypes.Where(type => type.IsSubclassOf(typeof(Section))).ToList();
            subtypes.ForEach(subtype => discriminatorBuilder.HasValue(subtype, subtype.Name));
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vecto.Application.Helpers;
using Vecto.Core.Entities;

namespace Vecto.Infrastructure.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            var discriminatorBuilder = builder.HasDiscriminator<string>(nameof(Section.SectionType));
            var subtypes = typeof(Section).GetSubtypesInSameAssembly();
            subtypes.ForEach(subtype => discriminatorBuilder.HasValue(subtype, subtype.Name));
        }
    }
}

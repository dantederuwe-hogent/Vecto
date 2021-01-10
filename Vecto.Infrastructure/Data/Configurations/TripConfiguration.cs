using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vecto.Core.Entities;

namespace Vecto.Infrastructure.Data.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasMany(t => t.Sections).WithOne();
        }
    }
}

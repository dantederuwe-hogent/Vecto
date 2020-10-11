using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Vecto.Core.Entities;

namespace Vecto.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public new DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>().ToTable("IdentityUsers");
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as EntityBase;
                entity.DateModified = DateTime.Now;
                if (entry.State == EntityState.Added) entity.DateCreated = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}

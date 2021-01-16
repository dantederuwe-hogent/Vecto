using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Infrastructure.Data.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly DbSet<Trip> _trips;
        private readonly AppDbContext _context;

        public TripRepository(AppDbContext context)
        {
            _context = context;
            _trips = _context.Trips;
        }

        public void Add(Trip trip) => _trips.Add(trip);
        public IList<Trip> GetAll() => _trips
            .Include(t => t.Sections)
            .ThenInclude(s => (s as PackingSection).Items)
            .Include(t => t.Sections)
            .ThenInclude(s => (s as TodoSection).Items)
            .ToList();
        public Trip GetBy(Guid id) => GetAll().SingleOrDefault(t => t.Id.Equals(id));
        public void Update(Trip trip) => _trips.Update(trip);
        public void Delete(Trip trip) => _trips.Remove(trip);
        public int SaveChanges() => _context.SaveChanges();
    }
}

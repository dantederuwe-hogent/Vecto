using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _users = _context.Users;
        }

        public void Add(User user) => _users.Add(user);
        public IList<User> GetAll() => _users.Include(u => u.Trips).ToList();
        public User GetBy(Guid id) => GetAll().SingleOrDefault(u => u.Id.Equals(id));
        public User GetBy(string email) => GetAll().SingleOrDefault(u => u.Email.Equals(email));
        public void Update(User user) => _users.Update(user);
        public void Delete(User user) => _users.Remove(user);
        public int SaveChanges() => _context.SaveChanges();
    }
}

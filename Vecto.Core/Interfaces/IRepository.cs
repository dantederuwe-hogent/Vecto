using System;
using System.Collections.Generic;
using Vecto.Core.Entities;

namespace Vecto.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        public void Add(T trip);
        public T GetBy(Guid id);
        public IList<T> GetAll();
        public void Update(T trip);
        public void Delete(T trip);
        public int SaveChanges();
    }
    public interface IUserRepository : IRepository<User>
    {
        public User GetBy(string email);
    }
}
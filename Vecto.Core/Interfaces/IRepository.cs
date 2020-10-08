using System;
using System.Collections.Generic;
using Vecto.Core.Entities;

namespace Vecto.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        public void Add(T item);
        public T GetBy(Guid id);
        public IList<T> GetAll();
        public void Update(T item);
        public void Delete(T item);
        public int SaveChanges();
    }
}
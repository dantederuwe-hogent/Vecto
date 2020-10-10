using Vecto.Core.Entities;

namespace Vecto.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetBy(string email);
    }
}
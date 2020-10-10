using Bogus;
using Vecto.Application.DTOs;
using Vecto.Core.Entities;

namespace Vecto.Tests.UnitTests
{
    public static class DummyData
    {
        public static readonly Faker<User> UserFaker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.Email, f => f.Person.Email);

        public static readonly Faker<RegisterDTO> RegisterDTOFaker = new Faker<RegisterDTO>()
            .RuleFor(r => r.FirstName, f => f.Person.FirstName)
            .RuleFor(r => r.LastName, f => f.Person.LastName)
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.Password, f => f.Internet.Password(8))
            .RuleFor(r => r.ConfirmPassword, (f, r) => r.Password);

        public static readonly Faker<LoginDTO> LoginDTOFaker = new Faker<LoginDTO>()
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.Password, f => f.Internet.Password(8));
    }
}

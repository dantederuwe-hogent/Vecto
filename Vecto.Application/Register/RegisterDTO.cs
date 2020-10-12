using Vecto.Application.Login;

namespace Vecto.Application.Register
{
    public class RegisterDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

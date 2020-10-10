namespace Vecto.Application.DTOs
{
    public class RegisterDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

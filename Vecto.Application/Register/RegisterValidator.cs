using FluentValidation;
using Vecto.Application.DTOs;
using Vecto.Application.Login;

namespace DamianTourBackend.Application.Register
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            Include(new LoginValidator());

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(200).WithMessage("First name cannot be longer than 200 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(200).WithMessage("Last name cannot be longer than 200 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation is required")
                .MinimumLength(8).WithMessage("Password should be a minimum of 8 characters long")
                .Equal(x => x.Password).WithMessage("Passwords should match.");
        }
    }
}

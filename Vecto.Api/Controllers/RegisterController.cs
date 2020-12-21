using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Vecto.Api.Helpers;
using Vecto.Application.Register;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IValidator<RegisterDTO> _registerValidator;

        public RegisterController(
            IUserRepository userRepository,
            IConfiguration config,
            UserManager<IdentityUser> userManager,
            IValidator<RegisterDTO> registerValidator
        )
        {
            _configuration = config;
            _userManager = userManager;
            _registerValidator = registerValidator;
            _userRepository = userRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var validation = await _registerValidator.ValidateAsync(model);
            if (!validation.IsValid) return BadRequest(validation);

            var existingUser = _userRepository.GetBy(model.Email);
            if (existingUser != null) return BadRequest();

            var identityUser = model.MapToIdentityUser();
            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (!result.Succeeded) return BadRequest();

            var user = model.MapToUser();
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            string token = TokenHelper.GetToken(identityUser, _configuration);
            return Created("", token);
        }
    }
}

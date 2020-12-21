using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Vecto.Api.Helpers;
using Vecto.Application.Login;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginController(
            IValidator<LoginDTO> loginValidator,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
        )
        {
            _loginValidator = loginValidator;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var validation = await _loginValidator.ValidateAsync(model);
            if (!validation.IsValid) return BadRequest(validation);

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null) return BadRequest();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return BadRequest();

            string token = TokenHelper.GetToken(user, _configuration);
            return Ok(token);
        }
    }
}
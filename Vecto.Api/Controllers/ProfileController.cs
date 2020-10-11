using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vecto.Application.Profile;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ProfileController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public IActionResult GetProfile()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            string email = User.Identity.Name;
            if (email == null) return BadRequest();

            var user = _userRepository.GetBy(email);
            if (user == null) return BadRequest();

            return Ok(user.MapToDTO());
        }
    }
}

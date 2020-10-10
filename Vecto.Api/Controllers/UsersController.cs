using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Vecto.Api.Helpers;
using Vecto.Application.DTOs;
using Vecto.Application.Register;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<RegisterDTO> _registerValidator;


        public UsersController(IUserRepository userRepository, IConfiguration config, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IValidator<LoginDTO> loginValidator, IValidator<RegisterDTO> registerValidator)
        {
            _configuration = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _userRepository = userRepository;
        }


        /// <summary> Register a user </summary>
        /// <param name="model">the user details</param>
        /// <returns>A token</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var validation = _registerValidator.Validate(model);
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

        /// <summary> Login </summary>
        /// <param name="model">the login details</param>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var validation = _loginValidator.Validate(model);
            if (!validation.IsValid) return BadRequest(validation);

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null) return BadRequest();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return BadRequest();

            string token = TokenHelper.GetToken(user, _configuration);
            return Ok(token);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _userRepository.GetBy(id);
            return user == null ? (IActionResult)NotFound() : Ok(user);
        }
    }
}

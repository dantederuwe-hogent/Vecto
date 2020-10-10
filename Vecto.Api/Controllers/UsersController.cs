using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public UsersController(IUserRepository userRepository, IConfiguration config)
        {
            _configuration = config;
            _userRepository = userRepository;
        }


        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _userRepository.GetBy(id);
            return user == null ? (IActionResult)NotFound() : Ok(user);
        }
    }
}

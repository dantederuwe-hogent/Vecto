using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vecto.Api.Helpers;
using Vecto.Application.Register;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;
using Vecto.Infrastructure.Data;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _dbContext;
        
        public DevController(IUserRepository userRepository, UserManager<IdentityUser> userManager, IConfiguration configuration, AppDbContext dbContext)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        
        [HttpGet("allUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(new
            {   
                users.Count,
                users
            });
        }
        
        [HttpPost("seeduser")]
        public async Task<IActionResult> SeedUser()
        {
            var model = DummyData.RegisterDTOFaker.Generate();
            
            var identityUser = model.MapToIdentityUser();
            var result = await _userManager.CreateAsync(identityUser, model.Password);
            
            if (!result.Succeeded) return BadRequest();

            var user = model.MapToUser();
            ((List<Trip>) user.Trips).AddRange(DummyData.TripFaker.GenerateBetween(2, 6));
            
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            string token = TokenHelper.GetToken(identityUser, _configuration, true);
            return Created("", new
            {
                Token = token,
                Login = new {
                    user.Email,
                    model.Password,
                },
                user
            });
        }

        [HttpPost("cleardatabase/{sure}")]
        public IActionResult ClearDb(bool sure)
        {
            if (!sure) return BadRequest("you should be sure about this");
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            return Ok("it has been done");
        }
        
        [HttpPost("deletedatabase/{sure}")]
        public IActionResult DeleteDatabase(bool sure)
        {
            if (!sure) return BadRequest("you should be sure about this");
            _dbContext.Database.EnsureDeleted();
            return Ok("it has been done, please restart your application!");
        }
        
        [HttpPost("migratedatabase/{sure}")]
        public IActionResult MigrateDatabase(bool sure)
        {
            if (!sure) return BadRequest("you should be sure about this");
            _dbContext.Database.Migrate();
            return Ok("it has been done");
        }
    }
}
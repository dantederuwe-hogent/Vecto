using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vecto.Application.Trips;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TripsController : ControllerBase
    {
        private readonly ITripRepository _tripsRepository;
        private readonly IUserRepository _userRepository;

        public TripsController(ITripRepository tripsRepository, IUserRepository userRepository)
        {
            _tripsRepository = tripsRepository;
            _userRepository = userRepository;
        }
        
        [Route("")]
        public IActionResult Get()
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            return Ok(user.Trips);
        }
        
 
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody]TripDTO model)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            user.Trips.Add(model.MapToTrip());
            _userRepository.Update(user);
            _userRepository.SaveChanges();
            return RedirectToAction("Get");
        }
        
    }
}
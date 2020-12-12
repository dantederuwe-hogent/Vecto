using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Vecto.Application.Trips;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/trips/{tripId}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SectionsController : ControllerBase
    {
        private readonly ITripRepository _tripsRepository;
        private readonly IUserRepository _userRepository;

        public SectionsController(ITripRepository tripsRepository, IUserRepository userRepository)
        {
            _tripsRepository = tripsRepository;
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public IActionResult GetAll(Guid tripId)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user == null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(tripId));
            if (trip == null) return BadRequest();

            return Ok(trip.Sections);
        }

        [HttpGet("name")]
        public IActionResult GetBy(Guid tripId, string name)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user == null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(tripId));
            if (trip == null) return BadRequest();

            var section = trip.Sections.SingleOrDefault(s => s.Name.Equals(name));
            if (section == null) return BadRequest();

            return Ok(trip.Sections);
        }

        [HttpPost("")]
        public IActionResult Add(Guid tripId, string name, bool todo)
        {
            Section section;
            if (todo) section = new PackingSection() { Name = name };
            else section = new TodoSection() { Name = name };

            var trip = _tripsRepository.GetBy(tripId);
            trip.Sections.Add(section);

            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return RedirectToAction("GetBy", new { tripId });
        }
    }
}

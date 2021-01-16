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

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            return Ok(user.Trips);
        }

        [HttpGet("{id}")]
        public IActionResult GetBy(Guid id)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(id));
            if (trip is null) return BadRequest();

            return Ok(trip);
        }

        [HttpPost("")]
        public IActionResult Add([FromBody] TripDTO model)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            user.Trips.Add(model.MapToTrip());

            _userRepository.Update(user);
            _userRepository.SaveChanges();

            return Ok(user.Trips);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, [FromBody] TripDTO model)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(id));
            if (trip is null) return BadRequest();

            trip.UpdateWith(model);

            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return Ok(trip);
        }

        [HttpGet("{id}/progress")]
        public IActionResult GetProgress(Guid id)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = _tripsRepository.GetBy(id);
            if (trip is null) return NotFound("trip not found");

            // count items and checked items
            int items = 0, checkedItems = 0;
            trip.Sections.ToList().ForEach(s =>
            {
                if (s is TodoSection todoSection) todoSection.Items.ToList().ForEach(i =>
                {
                    items++;
                    if (i.Checked) checkedItems++;
                });
                else if (s is PackingSection packingSection) packingSection.Items.ToList().ForEach(i =>
                {
                    items++;
                    if (i.Checked) checkedItems++;
                });
            });

            // prevent division by 0
            if (items == 0) return Ok(0);

            // return progress [0, 1]
            return Ok((float)checkedItems / (float)items);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IList<Trip>> GetAll()
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user == null) return BadRequest();
            
            return Ok(user.Trips);
        }
        
        [HttpGet("{id}")]
        public ActionResult<IList<Trip>> GetBy(Guid id)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user == null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(id));
            if (trip == null) return BadRequest();
            
            return Ok(trip);
        }

        [HttpPost("")]
        public ActionResult<IList<TripDTO>> Add([FromBody]TripDTO model)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            user.Trips.Add(model.MapToTrip());
            _userRepository.Update(user);
            _userRepository.SaveChanges();
            return RedirectToAction("GetAll");
        }

        [HttpPatch("{id}")]
        public ActionResult<Trip> Update(Guid id, [FromBody] TripDTO model)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user == null) return BadRequest();

            var trip = user.Trips.SingleOrDefault(t => t.Id.Equals(id));
            if (trip == null) return BadRequest();

            trip.UpdateWith(model);
            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return Ok(trip);
        }
    }
}
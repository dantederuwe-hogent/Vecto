using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Vecto.Application.Helpers;
using Vecto.Application.Sections;
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
        private readonly IValidator<SectionDTO> _sectionValidator;

        public SectionsController(
            ITripRepository tripsRepository,
            IUserRepository userRepository,
            IValidator<SectionDTO> sectionValidator
        )
        {
            _tripsRepository = tripsRepository;
            _userRepository = userRepository;
            _sectionValidator = sectionValidator;
        }

        [HttpGet("")]
        public IActionResult GetAll(Guid tripId)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return BadRequest();

            return Ok(trip.Sections);
        }

        [HttpGet("{sectionId}")]
        public IActionResult GetBy(Guid tripId, Guid sectionId)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound($"trip with id {tripId} not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id.Equals(sectionId));
            if (section is null) return NotFound($"section with id {sectionId} not found in trip with id {tripId}");

            return Ok(section);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(Guid tripId, [FromBody] SectionDTO model)
        {
            var validation = await _sectionValidator.ValidateAsync(model);
            if (!validation.IsValid) return BadRequest(validation);

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = model.MapToSection();
            trip.Sections.Add(section);

            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return Ok(trip.Sections);
        }

        [HttpGet("/api/sections/types")]
        [AllowAnonymous]
        public IEnumerable<string> GetTypeNames()
        {
            return typeof(Section).GetSubtypesInSameAssembly().Select(s => s.Name);
        }
    }
}

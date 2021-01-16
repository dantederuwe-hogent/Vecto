using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Vecto.Application.Items;
using Vecto.Core.Entities;
using Vecto.Core.Interfaces;

namespace Vecto.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/trips/{tripId}/sections/{sectionId}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController : ControllerBase
    {
        private readonly ITripRepository _tripsRepository;
        private readonly IUserRepository _userRepository;

        public ItemsController(
            ITripRepository tripsRepository,
            IUserRepository userRepository
        )
        {
            _tripsRepository = tripsRepository;
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public IActionResult GetAll(Guid tripId, Guid sectionId)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            if (section is TodoSection todoSection) return Ok(todoSection.Items);
            else if (section is PackingSection packingSection) return Ok(packingSection.Items);

            return BadRequest();
        }

        [HttpGet("{itemId}")]
        public IActionResult GetBy(Guid tripId, Guid sectionId, Guid itemId)
        {
            var user = _userRepository.GetBy(User.Identity.Name);
            if (user is null) return BadRequest();

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            if (section is TodoSection todoSection) return Ok(todoSection.Items.SingleOrDefault(i => i.Id == itemId));
            else if (section is PackingSection packingSection) return Ok(packingSection.Items.SingleOrDefault(i => i.Id == itemId));
            else return NotFound("item not found");
        }

        [HttpPost("")]
        public IActionResult Add(Guid tripId, Guid sectionId, [FromBody] ItemDTO itemDTO)
        {
            // todo: validation

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            IActionResult res;
            switch (section)
            {
                case TodoSection ts:
                    var todo = itemDTO.MapToTodoItem();
                    ts.Items.Add(todo);
                    res = Ok(todo);
                    break;
                case PackingSection ps:
                    var pack = itemDTO.MapToPackingItem();
                    ps.Items.Add(pack);
                    res = Ok(pack);
                    break;
                default:
                    res = BadRequest();
                    return res;
            }

            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return res;
        }

        [HttpPatch("{itemId}")]
        public IActionResult Update(Guid tripId, Guid sectionId, Guid itemId, ItemDTO itemDTO)
        {
            // todo: validation

            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            ISectionItem item = null;
            if (section is TodoSection todoSection) item = todoSection.Items.SingleOrDefault(i => i.Id == itemId);
            else if (section is PackingSection packingSection) item = packingSection.Items.SingleOrDefault(i => i.Id == itemId);

            if (item is null) return NotFound("item not found");

            item.UpdateWith(itemDTO);

            _tripsRepository.Update(trip);
            _tripsRepository.SaveChanges();

            return Ok();
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(Guid tripId, Guid sectionId, Guid itemId)
        {
            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            if (section is TodoSection todoSection)
            {
                ISectionItem item = todoSection.Items.SingleOrDefault(i => i.Id == itemId);
                if (item is null) return NotFound("item not found");
                todoSection.Items.Remove((TodoItem)item);

            }
            else if (section is PackingSection packingSection)
            {
                ISectionItem item = packingSection.Items.SingleOrDefault(i => i.Id == itemId);
                if (item is null) return NotFound("item not found");
                packingSection.Items.Remove((PackingItem) item);
            }

            _tripsRepository.SaveChanges();

            return Ok();
        }

        [HttpPost("{itemId}/toggle")]
        public IActionResult Toggle(Guid tripId, Guid sectionId, Guid itemId)
        {
            var trip = _tripsRepository.GetBy(tripId);
            if (trip is null) return NotFound("trip not found");

            var section = trip.Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section is null) return NotFound("section not found");

            ISectionItem item = null;
            if (section is TodoSection todoSection) item = todoSection.Items.SingleOrDefault(i => i.Id == itemId);
            else if (section is PackingSection packingSection) item = packingSection.Items.SingleOrDefault(i => i.Id == itemId);

            if (item is null) return NotFound("item not found");

            if (item is IToggleable tItem) tItem.Toggle();

            _tripsRepository.SaveChanges();

            return Ok();
        }
    }
}

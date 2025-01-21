using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;
using System.Security.Claims;

namespace storage_management_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly PgContext _context;

        public LocationController(PgContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="HeadAdmin")]
        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDto locationDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (locationDto == null || string.IsNullOrWhiteSpace(locationDto.Address))
            {
                return BadRequest("Address is required.");
            }

            var newLocation = new Location
            {
                Address = locationDto.Address
            };

            var UserAction = new UserAction
            {
                Description = $"Location [{locationDto.Address}] added.",
                Time = DateTime.UtcNow,
                OperationId = 12,
                UserId = int.Parse(userIdClaim.Value),
            };

            _context.Locations.Add(newLocation);
            _context.UserActions.Add(UserAction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocationById), new { id = newLocation.Id }, newLocation);
        }

        [Authorize(Roles = "RootAdmin")]
        [HttpGet("GetLocationById")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [Authorize(Roles = "HeadAdmin")]
        [HttpPut("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto locationDto)
        {
            if (locationDto == null || string.IsNullOrWhiteSpace(locationDto.Address))
            {
                return BadRequest("Address is required.");
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound($"Location with ID {id} not found.");
            }

            location.Address = locationDto.Address;

            _context.Locations.Update(location);
            await _context.SaveChangesAsync();

            return Ok(location);
        }

        [Authorize(Roles = "HeadAdmin")]
        [HttpDelete("DeleteLocation")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound($"Location with ID {id} not found.");
            }

            var UserAction = new UserAction
            {
                Description = $"Location [{location.Address}] removed.",
                Time = DateTime.UtcNow,
                OperationId = 13,
                UserId = int.Parse(userIdClaim.Value),
            };

            _context.Locations.Remove(location);
            _context.UserActions.Add(UserAction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

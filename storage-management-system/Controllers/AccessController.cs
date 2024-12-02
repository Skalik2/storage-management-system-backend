using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;

namespace storage_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly ILogger<AccessController> _logger;

        private readonly PgContext _context;
        public AccessController(ILogger<AccessController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [HttpPost("AssignAccessToBox")]
        public async Task<IActionResult> AssignAccessToBox([FromBody] AssignAccessDto request)
        {
            if (request.BoxIds == null || !request.BoxIds.Any())
            {
                return BadRequest("BoxIds cannot be null or empty.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
            {
                return NotFound($"User with ID {request.UserId} not found.");
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                foreach (var boxId in request.BoxIds)
                {
                    var boxExists = await _context.Boxes.AnyAsync(b => b.Id == boxId);
                    if (!boxExists)
                    {
                        return NotFound($"Box with ID {boxId} not found.");
                    }

                    var exists = await _context.Accesses.AnyAsync(a => a.UserId == request.UserId && a.BoxId == boxId);
                    if (!exists)
                    {
                        var access = new Access
                        {
                            UserId = request.UserId,
                            BoxId = boxId,
                        };

                        await _context.Accesses.AddAsync(access);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Access successfully assigned to the provided boxes.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpDelete("RevokeAccessFromBox")]
        public async Task<IActionResult> RevokeAccessFromBox([FromBody] AssignAccessDto request)
        {
            if (request.BoxIds == null || !request.BoxIds.Any())
            {
                return BadRequest("BoxIds cannot be null or empty.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
            {
                return NotFound($"User with ID {request.UserId} not found.");
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                foreach (var boxId in request.BoxIds)
                {
                    var boxExists = await _context.Boxes.AnyAsync(b => b.Id == boxId);
                    if (!boxExists)
                    {
                        return NotFound($"Box with ID {boxId} not found.");
                    }

                    var access = await _context.Accesses
                        .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.BoxId == boxId);

                    if (access != null)
                    {
                        _context.Accesses.Remove(access);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Access successfully revoked from the provided boxes.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpDelete("RevokeAllAccessForUser")]
        public async Task<IActionResult> RevokeAllAccessForUser(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var accesses = await _context.Accesses.Where(a => a.UserId == userId).ToListAsync();

                if (accesses.Any())
                {
                    _context.Accesses.RemoveRange(accesses);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return Ok($"All accesses for user with ID {userId} have been revoked.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }
    }
}

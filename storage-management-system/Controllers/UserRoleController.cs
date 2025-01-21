using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace storage_management_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly PgContext _context;

        public UserRoleController(PgContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="RootAdmin")]
        [HttpPost("AddHeadAdminRole")]
        public async Task<IActionResult> AddHeadAdminRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var adminRoleExists = await _context.UserRole.AnyAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 2);
            if (adminRoleExists)
            {
                return Conflict("The user already has the head admin role.");
            }

            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = 2
            };

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"Head admin role assigned to userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 6,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();

                return Ok("HeadAdmin role added successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
        }

        [Authorize(Roles = "HeadAdmin")]
        [HttpPost("AddAdminRole")]
        public async Task<IActionResult> AddAdminRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var adminRoleExists = await _context.UserRole.AnyAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 3);
            if (adminRoleExists)
            {
                return Conflict("The user already has the admin role.");
            }

            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = 3
            };

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"Admin role assigned to userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 6,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                return Ok("Admin role added successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }

        }

        [Authorize(Roles ="HeadAdmin,Admin")]
        [HttpPost("AddServiceRole")]
        public async Task<IActionResult> AddServiceRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var adminRoleExists = await _context.UserRole.AnyAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 4);
            if (adminRoleExists)
            {
                return Conflict("The user already has the service role.");
            }

            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = 4
            };

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"Service role assigned to userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 6,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                return Ok("Service role added successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
        }

        [Authorize(Roles="RootAdmin")]
        [HttpDelete("RemoveHeadAdminRole")]
        public async Task<IActionResult> RemoveHeadAdminRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 2);
            if (userRole == null)
            {
                return NotFound("The user does not have the HeadAdmin role.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"HeadAdmin role removed from userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 7,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                return Ok("HeadAdmin role removed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
        }

        [Authorize(Roles ="HeadAdmin")]
        [HttpDelete("RemoveAdminRole")]
        public async Task<IActionResult> RemoveAdminRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 3);
            if (userRole == null)
            {
                return NotFound("The user does not have the Admin role.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"Admin role removed from userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 7,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                return Ok("Admin role removed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
        }

        [Authorize(Roles ="HeadAdmin,Admin")]
        [HttpDelete("RemoveServiceRole")]
        public async Task<IActionResult> RemoveServiceRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null || userRoleDto.UserId <= 0)
            {
                return BadRequest("Valid UserId is required.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userRoleDto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserId == userRoleDto.UserId && ur.RoleId == 4);
            if (userRole == null)
            {
                return NotFound("The user does not have the Service role.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var UserAction = new UserAction
            {
                Description = $"Service role removed from userId: {userRoleDto.UserId}",
                Time = DateTime.UtcNow,
                OperationId = 7,
                UserId = int.Parse(userIdClaim.Value),
            };

            try
            {
                _context.UserRole.Add(userRole);
                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                return Ok("Service role removed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
        }
    }
}

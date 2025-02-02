﻿using Microsoft.AspNetCore.Authorization;
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
    public class AccessController : ControllerBase
    {
        private readonly ILogger<AccessController> _logger;

        private readonly PgContext _context;
        public AccessController(ILogger<AccessController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [Authorize(Roles ="HeadAdmin,Admin")]
        [HttpPost("AssignAccessToBox")]
        public async Task<IActionResult> AssignAccessToBox([FromBody] AssignAccessDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

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
                var UserAction = new UserAction
                {
                    Description = $"Access assigned to box/es for userId {request.UserId}.",
                    Time = DateTime.UtcNow,
                    OperationId = 8,
                    UserId = int.Parse(userIdClaim.Value),
                };

                _context.UserActions.Add(UserAction);
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

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpDelete("RevokeAccessFromBox")]
        public async Task<IActionResult> RevokeAccessFromBox([FromBody] AssignAccessDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

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

                var UserAction = new UserAction
                {
                    Description = $"Access to box/es removed from userId {request.UserId}.",
                    Time = DateTime.UtcNow,
                    OperationId = 9,
                    UserId = int.Parse(userIdClaim.Value),
                };

                _context.UserActions.Add(UserAction);
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

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpDelete("RevokeAllAccessForUser")]
        public async Task<IActionResult> RevokeAllAccessForUser(int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

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

                var UserAction = new UserAction
                {
                    Description = $"All access to box/es removed from userId {userId}.",
                    Time = DateTime.UtcNow,
                    OperationId = 9,
                    UserId = int.Parse(userIdClaim.Value),
                };

                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok($"All accesses for user with ID {userId} have been revoked.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpPost("GrantAccessToAllBoxes")]
        public async Task<IActionResult> GrantAccessToAllBoxes([FromBody] GrantFullAccessDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var sql = @"
                    CALL grant_access_to_all_boxes(@userId, @companyId, @storageId);
                ";

                var userIdParam = new Npgsql.NpgsqlParameter("@userId", request.UserId);
                var companyIdParam = new Npgsql.NpgsqlParameter("@companyId", request.CompanyId);
                var storageIdParam = new Npgsql.NpgsqlParameter("@storageId", request.StorageId);

                await _context.Database.ExecuteSqlRawAsync(sql, userIdParam, companyIdParam, storageIdParam);

                var UserAction = new UserAction
                {
                    Description = $"Access assigned to all box/es for userId {request.UserId}.",
                    Time = DateTime.UtcNow,
                    OperationId = 8,
                    UserId = int.Parse(userIdClaim.Value),
                };

                _context.UserActions.Add(UserAction);
                await _context.SaveChangesAsync();

                return Ok("Access granted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpGet("GetAllUserBoxes")]
        public async Task<IActionResult> GetAllUserBoxes(int userId)
        {
            var boxes = await _context.Accesses
            .Where(a => a.UserId == userId)
            .Select(a => new
            {
                a.BoxId
            })
            .ToListAsync();

            if (boxes == null || boxes.Count == 0)
            {
                return NotFound($"User with ID {userId} has no access to any boxes.");
            }

            return Ok(boxes);
        }
    }
}

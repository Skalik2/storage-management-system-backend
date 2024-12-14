using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;

namespace storage_management_system.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserActionController : Controller
    {
        private readonly PgContext _context;
        public UserActionController(PgContext pgContext)
        {
            _context = pgContext;
        }

        [HttpGet("GetUserActionsByUserId")]
        public async Task<IActionResult> GetUserActionsByUserId(int userId)
        {
            var userActions = await _context.UserActions
                .Where(ua => ua.UserId == userId)
                .Select(ua => new
                {
                    ua.Id,
                    ua.BoxId,
                    ua.OperationId,
                    ua.Quantity
                })
                .ToListAsync();

            if (userActions == null || !userActions.Any())
            {
                return NotFound($"No actions found for User ID {userId}.");
            }

            return Ok(userActions);
        }
    }
}

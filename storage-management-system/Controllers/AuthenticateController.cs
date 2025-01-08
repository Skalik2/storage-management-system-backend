using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;
using storage_management_system.Services;

namespace storage_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly JwtService _jwtService;

        private readonly PgContext _context;

        public AuthenticateController(JwtService jwtService, PgContext pgContext)
        {
            _jwtService = jwtService;
            _context = pgContext;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var result = await _jwtService.Authenticate(loginRequest);
            if (result == null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == loginRequest.UserName);
                if (user == null) {
                    return Unauthorized();
                }
                var UserActionFailedLogin = new UserAction
                {
                    Description = $"Login atempt failed to user: {loginRequest.UserName}",
                    Time = DateTime.UtcNow,
                    OperationId = 1,
                    UserId = user.Id,
                };

                _context.UserActions.Add(UserActionFailedLogin);
                _context.SaveChanges();

                return Unauthorized(UserActionFailedLogin.Description);
            }

            var UserActionSuccessLogin = new UserAction
            {
                Description = $"Login successful: {loginRequest.UserName}",
                Time = DateTime.UtcNow,
                OperationId = 1,
            };

            _context.UserActions.Add(UserActionSuccessLogin);
            _context.SaveChanges();

            return Ok(result);
        }
    }
}

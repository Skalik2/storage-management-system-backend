using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Services;

namespace storage_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthenticateController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var result = await _jwtService.Authenticate(loginRequest);
            if (result == null) 
                return Unauthorized();
            return Ok(result);
        }
    }
}

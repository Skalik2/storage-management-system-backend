using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.Entities;
using storage_management_system.Model.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using storage_management_system.Services;

namespace storage_management_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private IPasswordHasher _passwordHasher;
        private readonly PgContext _context;

        public UsersController(ILogger<UsersController> logger, PgContext pgContext, IPasswordHasher passwordHasher)
        {
            _logger = logger;
            _context = pgContext;
            _passwordHasher = passwordHasher;
            
        }

        [Authorize(Roles = "RootAdmin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var allUsers = await _context.Users.ToListAsync();

            return Ok(allUsers);
        }

        [Authorize(Roles = "RootAdmin")]
        [HttpGet("GetUserById")]
        public async Task<ActionResult<List<User>>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");

            return Ok(user);
        }

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpPost("PostBasicUser")]
        public async Task<IActionResult> PostBasicUser([FromBody] UserCreateBasicDto userDto)
        {
            if (userDto == null)
            { 
                return BadRequest("User data is null.");
            }

            var company = await _context.Companies.FindAsync(userDto.CompanyId);
            if (company == null)
            {
                return NotFound($"Company with ID {userDto.CompanyId} not found.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email || u.Username == userDto.Username);

            if (existingUser != null)
            {
                if (existingUser.Email == userDto.Email)
                {
                    return Conflict($"A user with the email '{userDto.Email}' already exists.");
                }
                if (existingUser.Username == userDto.Username)
                {
                    return Conflict($"A user with the username '{userDto.Username}' already exists.");
                }
            }

            User user = new()
            {
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = _passwordHasher.Hash(userDto.Password),
                CompanyId = userDto.CompanyId,
                Company = _context.Companies.Find(userDto.CompanyId),
            };

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpPost("PostFullUser")]
        public async Task<IActionResult> PostFullUser([FromBody] UserCreateFullDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null.");
            }

            var company = await _context.Companies.FindAsync(userDto.CompanyId);
            if (company == null)
            {
                return NotFound($"Company with ID {userDto.CompanyId} not found.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email || u.Username == userDto.Username);

            if (existingUser != null)
            {
                if (existingUser.Email == userDto.Email)
                {
                    return Conflict($"A user with the email '{userDto.Email}' already exists.");
                }
                if (existingUser.Username == userDto.Username)
                {
                    return Conflict($"A user with the username '{userDto.Username}' already exists.");
                }
            }

            User user = new()
            {
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                CompanyId = userDto.CompanyId,
                Company = _context.Companies.Find(userDto.CompanyId),
            };

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
    }
}

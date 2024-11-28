using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.Entities;
using storage_management_system.Model.DataTransferObject;

namespace storage_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        private readonly PgContext _context;
        public UsersController(ILogger<UsersController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [HttpGet(Name = "GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var allUsers = await _context.Users.ToListAsync();

            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserCreateBasicDto userDto)
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

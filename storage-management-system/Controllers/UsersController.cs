using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.Entities;

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
        public async Task<ActionResult<List<User>>> GetAll()
        {
            //var user = new User()
            //{
            //    Id = 1,
            //    Name = "Test",
            //    Email = "Test",
            //    Password = "Test",
            //};

            //_context.Add(user);
            //await _context.SaveChangesAsync();

            var allUsers = await _context.Users.ToListAsync();

            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> GetSingle(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found");

            return Ok(user);
        }
    }
}

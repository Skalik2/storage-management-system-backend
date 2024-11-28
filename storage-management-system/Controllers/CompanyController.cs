using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.Entities;

namespace storage_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;

        private readonly PgContext _context;
        public CompanyController(ILogger<CompanyController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [HttpGet("GetAllCompanies")]
        public async Task<ActionResult<List<Company>>> GetAllCompanies()
        {
            var allCompanies = await _context.Companies.ToListAsync();

            return Ok(allCompanies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetOneCompany(int id)
        {
            var singleCompany = await _context.Companies.FindAsync(id);

            return Ok(singleCompany);
        }
    }
}

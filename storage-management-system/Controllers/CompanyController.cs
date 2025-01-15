using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace storage_management_system.Controllers
{
    [Authorize]
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

        [Authorize(Roles ="RootAdmin")]
        [HttpGet("GetAllCompanies")]
        public async Task<ActionResult<List<Company>>> GetAllCompanies()
        {
            var allCompanies = await _context.Companies.ToListAsync();

            return Ok(allCompanies);
        }

        [Authorize(Roles ="RootAdmin")]
        [HttpGet("GetCompanyByID")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var singleCompany = await _context.Companies.FindAsync(id);

            return Ok(singleCompany);
        }

        [Authorize(Roles ="HeadAdmin,Admin")]
        [HttpGet("GetAllUsersByCompanyId")]
        public async Task<ActionResult<Company>> GetAllUsersByCompanyId(int id)
        {
            try
            {
                var company = await _context.Companies
                    .Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (company == null)
                {
                    return NotFound($"Company with id {id} not found.");
                }

                return Ok(company.Users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data: {ex.Message}");
            }
        }

        [Authorize(Roles ="HeadAdmin")]
        [HttpPut("UpdateCompanyName")]
        public async Task<IActionResult> UpdateCompanyName(int companyId, string newName)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL update_company_name({0}, {1});",
                    companyId,
                    newName);

                return Ok(new { message = "Company name updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating company name: {ex.Message}");
            }
        }
    }
}

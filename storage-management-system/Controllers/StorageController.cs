using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;

namespace storage_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;

        private readonly PgContext _context;
        public StorageController(ILogger<StorageController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [HttpPost("CreatePredefinedStorage")]
        public async Task<IActionResult> CreatePredefinedStorage([FromBody] PredefinedStorageDto request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"CALL create_predefined_storage_structure({0}, {1}, {2});",
                    request.CompanyId,
                    request.LocationId,
                    request.Model
                );

                return Ok("Storage structure created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpPost("CreateCustomStorage")]
        public async Task<IActionResult> CreateCustomStorage([FromBody] CustomStorageDto request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"CALL create_custom_storage_structure({0}, {1}, {2}, {3}, {4});",
                    request.CompanyId,
                    request.LocationId,
                    request.RowCount,
                    request.SectionCount,
                    request.BoxCount
                );

                return Ok("Custom storage structure created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteStorageById")]
        public async Task<IActionResult> DeleteStorage(int storageId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"CALL delete_storage_by_id({0});",
                    storageId
                );

                return Ok("Storage and its associated structure have been deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetRowsIdsByStorageId")]
        public async Task<IActionResult> GetRowsIdsByStorageId(int storageId)
        {
            try
            {
                var rowIds = await _context.Rows
                    .Where(r => r.StorageId == storageId)
                    .Select(r => r.Id)
                    .ToListAsync();

                if (rowIds == null || !rowIds.Any())
                {
                    return NotFound("No rows found for the specified storage.");
                }

                return Ok(rowIds);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetSectionsIdsByStorageId")]
        public async Task<IActionResult> GetSectionsIdsByStorageId(int storageId)
        {
            try
            {
                var sectionIds = await _context.Sections
                    .Where(s => s.Row.StorageId == storageId)
                    .Select(s => s.Id)
                    .ToListAsync();

                if (sectionIds == null || !sectionIds.Any())
                {
                    return NotFound("No sections found for the specified storage.");
                }

                return Ok(sectionIds);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetBoxesIdByStorageId")]
        public async Task<IActionResult> GetBoxesIdsForStorage(int storageId)
        {
            try
            {
                var boxIds = await _context.Boxes
                    .Where(b => b.Section.Row.StorageId == storageId)
                    .Select(b => b.Id)
                    .ToListAsync();

                if (boxIds == null || !boxIds.Any())
                {
                    return NotFound("No boxes found for the specified storage.");
                }

                return Ok(boxIds);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetAllStorages")]
        public async Task<IActionResult> GetAllStorages()
        {
            try
            {
                var storages = await _context.Storages.ToListAsync();

                if (storages == null || !storages.Any())
                {
                    return NotFound("No storages found.");
                }

                return Ok(storages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

    }
}

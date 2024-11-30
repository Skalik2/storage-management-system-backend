﻿using Microsoft.AspNetCore.Mvc;
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
    }
}

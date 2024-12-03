using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;

namespace storage_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        private readonly PgContext _context;
        public ItemController(ILogger<ItemController> logger, PgContext pgContext)
        {
            _logger = logger;
            _context = pgContext;
        }

        [HttpPost]
        [Route("CreateItem")]
        public async Task<IActionResult> CreateItem([FromForm] ItemPictureDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("Invalid Submission!");
            }

            var newItem = new Item
            {
                Name = model.Name,
                Description = string.IsNullOrWhiteSpace(model.Description) ? "No description" : model.Description
            };

            _context.Add(newItem);
            await _context.SaveChangesAsync();

            var itemId = newItem.Id;

            if (model.Picture == null)
            {
                return Ok($"Item '{newItem.Name}' created without image!");
            }

            foreach (var picture in model.Picture)
            {
                if (picture == null || picture.Length == 0)
                {
                    return BadRequest("File not selected or empty!");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                var newPicture = new ItemPicture    
                {
                    ItemId = itemId,
                    ImageName = picture.FileName,
                    ImagePath = $"/Images/{uniqueFileName}"
                };

                _context.Add(newPicture);
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Item created successfully!", ItemId = itemId });
        }
    }
}

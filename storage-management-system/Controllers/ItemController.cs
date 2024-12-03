using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpDelete]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound($"Item with ID {itemId} not found.");
            }

            var itemPictures = await _context.ItemPictures.Where(p => p.ItemId == itemId).ToListAsync();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            foreach (var picture in itemPictures)
            {
                var fullPath = Path.Combine(uploadsFolder, picture.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.ItemPictures.RemoveRange(itemPictures);

            _context.Items.Remove(item);

            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Item with ID {itemId} and its associated pictures have been deleted." });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using storage_management_system.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace storage_management_system.Controllers
{
    [Authorize]
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


        [Authorize(Roles ="HeadAdmin,Admin,Service")]
        [HttpPost("CreateItem")]
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

        [Authorize(Roles = "HeadAdmin,Admin")]
        [HttpDelete("DeleteItem")]
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

        [HttpGet("GetItemsAccessibleByUser")]
        public async Task<ActionResult<IEnumerable<ItemInstance>>> GetItemsAccessibleByUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var accessibleBoxes = await _context.Accesses
                    .Where(a => a.UserId == userId)
                    .Select(a => a.BoxId)
                    .ToListAsync();

                if (!accessibleBoxes.Any())
                {
                    return NotFound($"No accessible boxes found for the user with ID {userId}.");
                }

                var items = await _context.ItemInstances
                    .Where(ii => accessibleBoxes.Contains(ii.BoxId))
                    .ToListAsync();

                if (!items.Any())
                {
                    return NotFound($"No items found in accessible boxes for the user with ID {userId}.");
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data for user ID: {ex.Message}");
            }
        }

        [Authorize(Roles ="HeadAdmin,Admin,Service")]
        [HttpPost("AssignItemToBox")]
        public async Task<ActionResult> AssignItemToBox(int itemId, int boxId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero.");
                }

                var boxExists = await _context.Boxes.AnyAsync(b => b.Id == boxId);
                if (!boxExists)
                {
                    return NotFound($"Box with ID {boxId} not found.");
                }

                var itemExists = await _context.Items.AnyAsync(i => i.Id == itemId);
                if (!itemExists)
                {
                    return NotFound($"Item with ID {itemId} not found.");
                }

                var existingItemInstance = await _context.ItemInstances
                    .FirstOrDefaultAsync(ii => ii.ItemId == itemId && ii.BoxId == boxId);

                if (existingItemInstance != null)
                {
                    existingItemInstance.Quantity += quantity;
                    _context.ItemInstances.Update(existingItemInstance);
                }
                else
                {
                    ItemInstance newItemInstance = new()
                    {
                        ItemId = itemId,
                        BoxId = boxId,
                        Quantity = quantity,
                        Box = _context.Boxes.Find(boxId),
                        Item = _context.Items.Find(itemId),
                    };

                    await _context.ItemInstances.AddAsync(newItemInstance);
                }

                await _context.SaveChangesAsync();

                return Ok("Item assigned to box successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error assigning item to box: {ex.Message}");
            }
        }
    }
}

using CheckBoxWebApi.Data;
using CheckBoxWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLayer.Dto;

namespace CheckBoxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : Controller
    { 
        private readonly CheckBoxDbContext _context;
        private readonly ILogger<GalleryController> _logger;

        public GalleryController(CheckBoxDbContext context, ILogger<GalleryController> logger) 
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Album), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            return album == null ? NotFound() : Ok(album);
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var albumDto = JsonConvert.DeserializeObject<AlbumDto>(HttpContext.Request.Form["json"]);

                if (httpRequest.Form.Files.Count > 0 && albumDto != null)
                {
                    var album = new Album()
                    {
                        UserId = albumDto.UserId,
                        Title = albumDto.Title,
                        Description = albumDto.Description,
                        ThumbnailName = albumDto.ThumbnailName,
                        FolderName = albumDto.FolderName,
                        CreationTime = albumDto.CreationTime
                    };
                    var result = await _context.Albums.AddAsync(album);
                    await _context.SaveChangesAsync();
                    foreach (var file in httpRequest.Form.Files)
                    {
                        var directory = Path.Combine("C:", "Uploads", album.UserId.ToString(), album.FolderName);

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream); 
                        System.IO.File.WriteAllBytes(Path.Combine(directory, file.FileName), memoryStream.ToArray());

                    }

                    return Ok();
                }
                return new StatusCodeResult(500);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Gallery Post controller");
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var albumToDelete = _context.Albums.Find(albumId);
            if (albumToDelete != null)
            {
                _context.Albums.Remove(albumToDelete);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateAlbum(AlbumDto albumDto)
        {
            var albumToUpdate = _context.Albums.Where(s => s.Id == albumDto.Id).FirstOrDefault();

            if (albumToUpdate != null)
            {
                albumToUpdate.Title = albumDto.Title ?? albumToUpdate.Title;
                albumToUpdate.Description = albumDto.Description ?? albumToUpdate.Description;
                albumToUpdate.EditTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }


        [HttpGet("byUserId/{userId}")]
        public async Task<IEnumerable<Album>> GetAlbumsByUserIdAsync(int userId)
        {
            var query = _context.Albums.Where(w => w.UserId == userId);
            return await query.ToListAsync();
        }

        [HttpPost("images/{albumId}")]
        public async Task AddImages(IEnumerable<string> fileNameList,[FromRoute] int albumId)
        {
            List<Image> images = new();

            foreach (var fileName in fileNameList)
            {
                images.Add(new Image { ImageName = fileName, AlbumId = albumId });
            }

            if (images.Any())
            {
                await _context.Images.AddRangeAsync(images);
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet("images")]
        public async Task<IEnumerable<Image>> GetImages(int albumId)
        {
            var query = _context.Images.Where(i => i.AlbumId == albumId);
            return await query.ToListAsync();
        }

        [HttpDelete("images")]
        public async Task<IActionResult> DeleteImages(IEnumerable<string> fileNameList, int userId, string folderName)
        {
            List<string> imageNames = new();
            var directory = Path.Combine(WebApiConstants.UploadsDisc, WebApiConstants.UploadsFolder, userId.ToString(), folderName);

            if (!Directory.Exists(directory))
            {
                return BadRequest($"Directory does not exist: {directory}");
            }

            foreach (var fileName in fileNameList)
            {
                var imagePath = Path.Combine(directory, fileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                imageNames.Add(fileName);
            }

            if (imageNames.Any())
            {
                var imagesToDelete = _context.Images.Where(i => imageNames.Contains(i.ImageName)).ToList();
                _context.Images.RemoveRange(imagesToDelete);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}

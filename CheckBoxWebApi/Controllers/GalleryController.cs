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
        private readonly IWebHostEnvironment _env;
        private readonly CheckBoxDbContext _context;

        public GalleryController(CheckBoxDbContext context, IWebHostEnvironment env) 
        {
            _env = env;
            _context = context;
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
                        ThumbnailUrl = albumDto.ThumbnailUrl,
                        FolderName = albumDto.FolderName,
                        CreationTime = albumDto.CreationTime
                    };
                    var result = await _context.Albums.AddAsync(album);
                    await _context.SaveChangesAsync();
                    foreach (var file in httpRequest.Form.Files)
                    {
                        var filePath = Path.Combine(_env.ContentRootPath, "uploads", album.UserId.ToString(), album.FolderName);

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream); System.IO.File.WriteAllBytes(Path.Combine(filePath, file.FileName), memoryStream.ToArray());
                        }

                    }
                    return Ok();
                }
                return new StatusCodeResult(500);
            }
            catch (Exception e)
            {
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
    }
}

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

        [Route("albums")]
        [HttpGet]
        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            return await _context.Albums.ToListAsync();
        }

        [Route("albums/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByAlbumsIdAsync(long id)
        {
            var album = await _context.Albums.FindAsync(id);
            return album == null ? NotFound() : Ok(album);
        }


        [Route("checks")]
        [HttpGet]
        public async Task<IEnumerable<Check>> GetChecksAsync()
        {
            return await _context.Checks.ToListAsync();
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
                    return CreatedAtAction(nameof(GetByAlbumsIdAsync), new { id = result.Entity.Id }, result.Entity);
                }
                return new StatusCodeResult(500);
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "Error");
                return new StatusCodeResult(500);
            }            
        }
    }
}

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

        [HttpGet("{albumId}")]
        [ProducesResponseType(typeof(Album), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            var album = await _context.Albums.FindAsync(albumId);
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
                        CreationTime = DateTime.UtcNow
                    };

                    var result = await _context.Albums.AddAsync(album);
                    await _context.SaveChangesAsync();
                    List<string> imageNames = new();

                    foreach (var file in httpRequest.Form.Files)
                    {
                        var directory = Path.Combine(WebApiConstants.UploadsDisc, WebApiConstants.UploadsFolder, album.UserId.ToString(), album.FolderName);

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream); 
                        System.IO.File.WriteAllBytes(Path.Combine(directory, file.FileName), memoryStream.ToArray());
                        imageNames.Add(file.FileName);
                    }

                    await AddImages(imageNames, album.Id);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAlbum()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var albumDto = JsonConvert.DeserializeObject<AlbumDto>(HttpContext.Request.Form["json"]);

                var albumToUpdate = _context.Albums.Where(s => s.Id == albumDto.Id).FirstOrDefault();

                if (albumToUpdate != null)
                {
                    albumToUpdate.Title = albumDto.Title ?? albumToUpdate.Title;
                    albumToUpdate.Description = albumDto.Description ?? albumToUpdate.Description;
                    albumToUpdate.ThumbnailName = albumDto.ThumbnailName ?? albumToUpdate.ThumbnailName;
                    albumToUpdate.EditTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound();
                }

                if (httpRequest.Form.Files.Count > 0 && albumDto != null)
                {
                    List<string> imageNames = new();

                    foreach (var file in httpRequest.Form.Files)
                    {
                        var directory = Path.Combine(WebApiConstants.UploadsDisc, WebApiConstants.UploadsFolder, albumToUpdate.UserId.ToString(), albumToUpdate.FolderName);

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        System.IO.File.WriteAllBytes(Path.Combine(directory, file.FileName), memoryStream.ToArray());
                        imageNames.Add(file.FileName);
                    }

                    await AddImages(imageNames, albumToUpdate.Id);
                }

                await DeleteImages(albumDto.ImagesToRemove, albumDto.UserId, albumDto.FolderName);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Gallery Put controller");
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var albumToDelete = _context.Albums.Find(albumId);
            if (albumToDelete != null)
            {
                await DeleteImagesByAlbum(albumToDelete);
                _context.Albums.Remove(albumToDelete);
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

        [HttpPost("addImages/{albumId}")]
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

        [HttpGet("images/{albumId}")]
        public async Task<IEnumerable<Image>> GetImages(int albumId)
        {
            var query = _context.Images.Where(i => i.AlbumId == albumId);
            return await query.ToListAsync();
        }

        [HttpDelete("images")]
        public async Task<IActionResult> DeleteImages(IEnumerable<string> fileNameList, int userId, string folderName)
        {
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
            }

            if (fileNameList.Any())
            {
                var imagesToDelete = _context.Images.Where(i => fileNameList.Contains(i.ImageName)).ToList();
                _context.Images.RemoveRange(imagesToDelete);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpDelete("deleteImagesByAlbum")]
        public async Task<IActionResult> DeleteImagesByAlbum(Album album)
        {
            var images = await GetImages(album.Id);
            if (images.Any())
            {
                var directory = Path.Combine(WebApiConstants.UploadsDisc, WebApiConstants.UploadsFolder, album.UserId.ToString(), album.FolderName);

                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }

                var fileNameList = images.Select(i => i.ImageName).ToList();

                if (fileNameList.Any())
                {
                    var imagesToDelete = _context.Images.Where(i => fileNameList.Contains(i.ImageName)).ToList();
                    _context.Images.RemoveRange(imagesToDelete);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok();
        }
    }
}

using CheckBoxWebApi.Data;
using CheckBoxWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckBoxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : Controller
    {
        private readonly CheckBoxDbContext _context;

        public GalleryController(CheckBoxDbContext context) => _context = context;

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
        public async Task<IActionResult> AddAlbum(Album album)
        {
            // var userWithThatEmailExist = await GetByUserEmail(user.Email);

            //if (userWithThatEmailExist)
            //{
            //    return BadRequest("User already exist");
            //}

            var result = await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByAlbumsIdAsync), new { id = result.Entity.Id }, result.Entity);
        }
    }
}

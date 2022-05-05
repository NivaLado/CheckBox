using CheckBoxWebApi.Data;
using CheckBoxWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckBoxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly CheckBoxDbContext _context;

        public AppUserController(CheckBoxDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.AppUsers.ToListAsync();
        }

        [HttpGet("userId")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(AppUser user)
        {
            await _context.AppUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByUserId), new { userId = user.UserId }, user);
        }
    }
}

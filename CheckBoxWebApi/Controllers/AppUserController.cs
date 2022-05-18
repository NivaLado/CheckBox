using CheckBoxWebApi.Data;
using CheckBoxWebApi.Models;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(long id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<bool> GetByUserEmail(string email)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(user => user.Email == email);
            return user != null;
        }

        [HttpPost]

        public async Task<IActionResult> CreateUserIfEmailIsFreeAsync(AppUser user)
        {
            var userWithThatEmailExist = await GetByUserEmail(user.Email);

            if (userWithThatEmailExist)
            {
                return BadRequest("User already exist");
            }

            await _context.AppUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByUserId), new { userId = user.Id }, user);
        }
    }
}

using CheckBoxWebApi.Data;
using CheckBoxWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLayer.Dto;

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
        public async Task<IActionResult> GetByUserId(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("google/{email}/{vendorId}")]
        public async Task<AppUser> GetByUserEmailAndVendorIdForGoogle(string email, string vendorId)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(user => user.Email == email && user.VendorId == vendorId);
            return user;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLoginAsync([FromBody] RegisterDto registerDto)
        {
            var user = await GetByUserEmailAndVendorIdForGoogle(registerDto.Email, registerDto.VendorId);

            if (user != null)
            {
                return CreatedAtAction(nameof(GetByUserId), new { id = user.Id }, user.Id);
            }

            try
            {
                user = new AppUser()
                {
                    AuthorizationMethod = registerDto.AuthorizationMethod,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    AccountCreated = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    VendorId = registerDto.VendorId,
                    Name = registerDto.Name,
                    Surname = registerDto.Surname
                };

                var result = await _context.AppUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetByUserId), new { id = user.Id }, user.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> RigisterUserAsync([FromBody] RegisterDto registerDto)
        {
            var userWithThatEmailExist = await GetByUserEmail(registerDto.Email);

            if (userWithThatEmailExist)
            {
                return BadRequest($"User already with Email: {registerDto.Email} already exist.");
            }

            try
            {
                var user = new AppUser()
                {
                    AuthorizationMethod = registerDto.AuthorizationMethod,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    AccountCreated = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                };

                var result = await _context.AppUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetByUserId), new { id = user.Id }, user.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] RegisterDto registerDto)
        {
            var user = await GetByUserEmailAndPassword(registerDto.Email, registerDto.Password);

            if (user?.Id >0)
            {
                return Ok(user?.Id);
            }
            else
            {
                return BadRequest(0);
            }
        }

        private async Task<bool> GetByUserEmail(string email)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(user => user.Email == email);
            return user != null;
        }

        private async Task<AppUser> GetByUserEmailAndPassword(string email, string password)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(user => user.Email == email && user.Password == password);
            return user;
        }
    }
}

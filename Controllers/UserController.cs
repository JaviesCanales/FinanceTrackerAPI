using Microsoft.AspNetCore.Mvc;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.DTOs;

namespace FinanceTrackerAPI
{
    [ApiController]
    [Route("api/user")]

    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            })
            .ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] CreateUserDTO dto)
        {
            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            var result = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            
            return CreatedAtAction(nameof(GetUsers), result);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(user);
        }
        [HttpPut("{id}")]
        public IActionResult EditUser(int id, [FromBody] EditUserDTO dto)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            if (dto.Name != null)
            {
                user.Name = dto.Name.Trim();
            }
            if (dto.Email != null)
            {
                user.Email = dto.Email.ToLowerInvariant().Trim();
            }
            if (dto.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password); 
            } 
            _context.SaveChanges();
            
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return Ok(result);
            
        }
        
    }
}
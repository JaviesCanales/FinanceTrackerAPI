using Microsoft.AspNetCore.Mvc;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FinanceTrackerAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTransactions([FromQuery] string? type, string? category)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(type))
            {

                query = query.Where(t => t.Type.ToLower() == type.ToLower());
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(t => t.Category.ToLower() == category.ToLower());
            }
            return Ok(query.ToList());

            
        }


        [HttpPost]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction.Type.ToLower() != "expense" && transaction.Type.ToLower() != "income")
            {
                return BadRequest();
            } 
            if (transaction.Amount <= 0)
            {
                return BadRequest("Amount must be greater than 0.00.");
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            transaction.UserId = userId;

            _context.Transactions.Add(transaction);      
            _context.SaveChanges();
            return CreatedAtAction(
            nameof(GetId),
            new { id = transaction.Id },
            transaction
            );     
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            if (transaction.UserId != userId) 
            {
                return Forbid();
            }
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            if (transaction.UserId != userId)
            {
                return Forbid();
            }
            return Ok(transaction);
        }
        [HttpPatch("{id}")]
        public IActionResult EditTransaction(int id, [FromBody] Transaction updatedtransaction)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            if (transaction.UserId != userId) 
            {
                return Forbid();
            }
            if (updatedtransaction.Type.ToLower() != "expense" && updatedtransaction.Type.ToLower() != "income")
            {
                return BadRequest();
            } 
            if (updatedtransaction.Description != null)
            {
                transaction.Description = updatedtransaction.Description;
            }
            if (updatedtransaction.Amount <= 0)
            {
                return BadRequest("Amount must be greater than 0.00.");
            }
            if (updatedtransaction.Amount != null)
            {
                transaction.Amount = updatedtransaction.Amount;
            }
            if (updatedtransaction.Category != null)
            {
                transaction.Category = updatedtransaction.Category;
            }
            if (updatedtransaction.Type != null)
            {
                transaction.Type = updatedtransaction.Type;
            }
            _context.SaveChanges();
            return Ok(transaction);
        }
        
    }
   
}
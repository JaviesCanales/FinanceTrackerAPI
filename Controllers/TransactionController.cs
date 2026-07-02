using Microsoft.AspNetCore.Mvc;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace FinanceTrackerAPI.Controllers
{
    [ApiController]
    //[Authorize]
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
            var query = _context.Transactions.AsQueryable();
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
            if (transaction.Amount < 1)
            {
                return BadRequest();
            }
            _context.Transactions.Add(transaction);      
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTransactions), transaction);     
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTransactions), transaction);
        }

        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpPut("{id}")]
        public IActionResult EditTransaction(int id, [FromBody] Transaction updatedtransaction)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            if (updatedtransaction.Type.ToLower() != "expense" && updatedtransaction.Type.ToLower() != "income")
            {
                return BadRequest();
            } 
            if (updatedtransaction.Description != null)
            {
                transaction.Description = updatedtransaction.Description;
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
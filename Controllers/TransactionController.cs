using Microsoft.AspNetCore.Mvc;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Data;

namespace FinanceTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Transaction> GetTransactions()
        {
            return _context.Transactions.ToList();
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            _context.Transactions.Add(transaction);      
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTransactions), transaction);     
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
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
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

    }
   
}
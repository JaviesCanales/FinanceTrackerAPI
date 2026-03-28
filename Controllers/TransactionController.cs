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
        [HttpPut("{id}")]
        public IActionResult EditTransaction(int id, [FromBody] Transaction updatedtransaction)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            transaction.Description = updatedtransaction.Description;
            transaction.Amount = updatedtransaction.Amount;
            transaction.Category = updatedtransaction.Category;
            transaction.Type = updatedtransaction.Type;
            _context.SaveChanges();
            return Ok(transaction);
        }

    }
   
}
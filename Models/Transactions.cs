using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerAPI.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Type {get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerAPI.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Type {get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
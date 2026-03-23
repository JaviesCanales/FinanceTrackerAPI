using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Models;

namespace FinanceTrackerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
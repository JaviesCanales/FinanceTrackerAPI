using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerAPI.DTOs
{
   

    public class EditUserDTO
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models.Salary
{
    public class AddSalaryViewModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        [ForeignKey("Users")]
        public int Username { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "FromDate is required.")]
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
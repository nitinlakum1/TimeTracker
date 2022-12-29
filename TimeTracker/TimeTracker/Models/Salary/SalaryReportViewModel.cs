using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Salary
{
    public class SalaryReportViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Salary Month is required.")]
        public DateTime SalaryMonth { get; set; }

        public DateTime? SalaryDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }
    }
}
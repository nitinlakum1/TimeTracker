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

        [Required(ErrorMessage = "Basic Salary is required.")]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "Payable Amount is required.")]
        public decimal PayableAmount { get; set; }

        [Required(ErrorMessage = "Working Days is required.")]
        public int WorkingDays { get; set; }
    }
}
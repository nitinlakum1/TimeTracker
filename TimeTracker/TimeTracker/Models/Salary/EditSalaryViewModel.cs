using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models.Salary
{
    public class EditSalaryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "From Date is required.")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required.")]
        public DateTime? ToDate { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker_Data.Model
{
    public class SalaryReports
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public DateTime SalaryMonth { get; set; }

        [Required]
        public DateTime SalaryDate { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        [Required]
        public decimal PayableAmount { get; set; }

        [Required]
        public int WorkingDays { get; set; }

        public Users Users { get; set; }
    }
}

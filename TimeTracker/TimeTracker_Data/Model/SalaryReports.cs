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
        public DateTime SalaryDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public Users Users { get; set; }
    }
}

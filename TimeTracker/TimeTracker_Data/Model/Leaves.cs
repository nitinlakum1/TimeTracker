using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker_Model;

namespace TimeTracker_Data.Model
{
    public class Leaves
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public DateTime LeaveFromDate { get; set; }

        [Required]
        public DateTime LeaveToDate { get; set; }

        [Required]
        public DateTime ApplyDate { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public Status Status { get; set;}

        [Required]
        public bool IsPaid { get; set; }

        public Users Users { get; set; }
    }
}

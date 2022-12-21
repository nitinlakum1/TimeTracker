using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker_Data.Model
{
    public class ErrorLogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ErrorLocation { get; set; }

        [Required]
        public string StackTrace { get; set; }

        [Required]
        public int LineNumber { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Users Users { get; set; }
    }
}
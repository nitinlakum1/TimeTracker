using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker_Model;

namespace TimeTracker_Data.Model
{
    public class SystemLogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public LogTypes LogType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime LogTime { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string WiFiName { get; set; }

        public Users Users { get; set; }
    }
}
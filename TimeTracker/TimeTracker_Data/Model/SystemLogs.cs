using System.ComponentModel.DataAnnotations;
using TimeTracker_Model;

namespace TimeTracker_Data.Model
{
    public class SystemLogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LogTypes LogType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime LogTime { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class Resources
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DataId { get; set; }

        [Required]
        public string preferenceId { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
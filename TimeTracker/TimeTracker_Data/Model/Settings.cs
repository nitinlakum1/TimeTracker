using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
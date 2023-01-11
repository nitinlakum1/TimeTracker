using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class UpdateServices
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
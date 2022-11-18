using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class ResourcesRemarks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PreferenceId { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker_Data.Model
{
    public class ResourcesRemarks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Resources")]
        public string preferenceId { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public Resources Resources { get; set; }
    }
}
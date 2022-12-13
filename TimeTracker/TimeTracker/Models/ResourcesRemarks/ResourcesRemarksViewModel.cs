using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.ResourcesRemarks
{
    public class ResourcesRemarksViewModel
    {
        public string? preferenceId { get; set; }

        [Required(ErrorMessage = "Remarks is required.")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "DateTime is required.")]
        public DateTime DateTime { get; set; }
    }
}

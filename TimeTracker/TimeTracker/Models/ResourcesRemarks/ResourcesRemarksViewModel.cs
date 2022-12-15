using System.ComponentModel.DataAnnotations;
using TimeTracker_Model;

namespace TimeTracker.Models.ResourcesRemarks
{
    public class ResourcesRemarksViewModel
    {
        public string? PreferenceId { get; set; }

        [Required(ErrorMessage = "Remarks is required.")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "DateTime is required.")]
        public DateTime DateTime { get; set; }
        [Required(ErrorMessage = "ResourceStatus is required.")]
        public ResourceStatus ResourceStatus { get; set; }
    }
}

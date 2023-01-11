using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.UpdateService
{
    public class AddUpdateServiceViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Version is required.")]
        public string Version { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
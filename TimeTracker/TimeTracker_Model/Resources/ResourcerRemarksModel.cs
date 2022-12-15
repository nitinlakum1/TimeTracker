using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker_Model.Resources
{
    public class ResourcerRemarksModel
    {
        public int Id { get; set; }

        public string? PreferenceId { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public ResourceStatus? ResourceStatus { get; set; }
    }
}

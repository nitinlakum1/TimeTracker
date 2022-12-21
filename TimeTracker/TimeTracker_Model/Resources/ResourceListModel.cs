using Newtonsoft.Json;
using TimeTracker_Model.Setting;

namespace TimeTracker_Model.Resources
{
    public class ResourceListModel
    {
        public string id { get; set; }
        public string preferenceId { get; set; }
        public string? name { get; set; }
        public string? gender { get; set; }
        public string? mobile { get; set; }
        public string? email { get; set; }
        public int? workYears { get; set; }
        public string? designation { get; set; }
        public string? degree { get; set; }
        public string? birthDate { get; set; }
        public string? workStartDate { get; set; }
        public string? companyExperiences { get; set; }
        public string? city { get; set; }
        public DateTime CreatedOn { get; set; }
        public ResourceStatus? ResourceStatus { get; set; }
    }
}

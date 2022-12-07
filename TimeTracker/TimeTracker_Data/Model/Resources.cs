using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class Resources
    {
        public string id { get; set; }

        [Required]
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
    }
}
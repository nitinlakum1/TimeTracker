namespace TimeTracker_Model.Setting
{
    public class ResourceModel
    {
        public string id { get; set; }
        public string preferenceId { get; set; }
        public string Data { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int workYears { get; set; }
        public string designation { get; set; }
        public string degree { get; set; }
        public string? birthDate { get; set; }
        public string? workStartDate { get; set; }
        public string? companyExperiences { get; set; }
        public string city { get; set; }
        public List<Preferences> preferences { get; set; }
        public List<Experiences> experiences { get; set; }
    }

    public class Preferences
    {
        public string city { get; set; }
        public string channel { get; set; }
    }

    public class Experiences
    {
        public string companyName { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string channel { get; set; }
    }
}
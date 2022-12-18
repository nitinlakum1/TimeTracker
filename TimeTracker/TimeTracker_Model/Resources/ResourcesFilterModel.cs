namespace TimeTracker_Model.Resources
{
    public class ResourcesFilterModel : DatatableParamModel
    {
        public int? Experience { get; set; }
        public string? Designation { get; set; } = "";
        public string? City { get; set; } = "";
    }
}
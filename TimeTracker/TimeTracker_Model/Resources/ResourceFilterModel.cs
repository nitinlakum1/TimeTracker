namespace TimeTracker_Model.Resources
{
    public class ResourceFilterModel : DatatableParamModel
    {
        public int Experience { get; set; }
        public string? Designation { get; set; } = "";
        public string? City { get; set; } = "";
        public ResourceStatus? Status { get; set; }
    }
}
namespace TimeTracker_Model
{
    public class DatatableParamModel
    {
        public int DisplayStart { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SearchText { get; set; } = "";
        public string SortColumn { get; set; } = "";
        public string SortOrder { get; set; } = "asc";
    }
}
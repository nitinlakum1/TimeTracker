namespace TimeTracker_Model.SystemLog
{
    public class MonthlyReportFilterModel : DatatableParamModel
    {
        public int UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
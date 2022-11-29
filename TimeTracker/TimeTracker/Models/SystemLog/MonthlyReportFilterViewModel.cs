namespace TimeTracker.Models.SystemLog
{
    public class MonthlyReportFilterViewModel : DatatableParamViewModel
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
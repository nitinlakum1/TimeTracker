namespace TimeTracker_Model.Salary
{
    public class SalaryReportModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime SalaryDate { get; set; }

        public decimal Amount { get; set; }
    }
}
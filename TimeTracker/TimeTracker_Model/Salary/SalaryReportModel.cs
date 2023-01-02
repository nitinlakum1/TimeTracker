namespace TimeTracker_Model.Salary
{
    public class SalaryReportModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime SalaryMonth { get; set; }
        public DateTime SalaryDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal PayableAmount { get; set; }
        public int WorkingDays { get; set; }
    }
}
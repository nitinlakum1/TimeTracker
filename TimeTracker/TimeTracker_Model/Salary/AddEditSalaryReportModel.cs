namespace TimeTracker_Model.Salary
{
    public class AddEditSalaryReportModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime SalaryDate { get; set; }

        public decimal Amount { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Model.Salary
{
    public class AddEditSalaryReportModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SalaryMonth { get; set; }
        public DateTime SalaryDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal PayableAmount { get; set; }
        public int WorkingDays { get; set; }
    }
}
namespace TimeTracker_Model.Salary
{
    public class SalaryModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public decimal Salary { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
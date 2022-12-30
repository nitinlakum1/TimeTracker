namespace TimeTracker_Model.Leave
{
    public class LeaveModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime LeaveFromDate { get; set; }

        public DateTime LeaveToDate { get; set; }

        public DateTime ApplyDate { get; set; }

        public string Reason { get; set; }

        public Status Status { get; set; }

        public bool IsPaid { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.Helper;
using TimeTracker_Model;

namespace TimeTracker.Models.Leave
{
    public class LeaveListViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
        public bool IsPaid { get; set; }
        public int? PendingLeave { get; set; }

        public string LeaveStatusName
        {
            get
            {
                return Status.GetEnumDescription();
            }
        }
    }
}
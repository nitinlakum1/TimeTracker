using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker_Model;

namespace TimeTracker.Models.Leave
{
    public class AddLeaveViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Leave From Date is required.")]
        public DateTime LeaveFromDate { get; set; }

        [Required(ErrorMessage = "Leave To Date is required.")]
        public DateTime LeaveToDate { get; set; }

        [Required(ErrorMessage = "Apply Date is required.")]
        public DateTime ApplyDate { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Is Paid is required.")]
        public bool IsPaid { get; set; }
    }
}
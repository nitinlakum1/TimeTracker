﻿namespace TimeTracker_Model.Salary
{
    public class AddEditSalaryModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public decimal Salary { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
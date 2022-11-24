using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Model.Holiday
{
    public class HolidayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
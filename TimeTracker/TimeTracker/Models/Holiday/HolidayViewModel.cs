using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Holiday
{
    public class HolidayViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Holiday
{
    public class HolidayViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Holiday is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
    }
}
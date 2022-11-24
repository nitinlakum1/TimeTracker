using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class Holidays
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
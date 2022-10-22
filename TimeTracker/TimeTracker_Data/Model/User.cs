using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
    }
}
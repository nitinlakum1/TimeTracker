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
        
        public string? Designation { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Address { get; set; }

        public DateTime? DOB { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        public string? BankName { get; set; }

        public string? AccountNo { get; set; }

        public string? IFSC { get; set; }
    }
}
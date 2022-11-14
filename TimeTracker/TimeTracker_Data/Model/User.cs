using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Data.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is Required!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should containt between 5 to 20 characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FullName is Required!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No. is required!")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a valid Contact No.!")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Gender is Required!")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        public string Password { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }
    }
}
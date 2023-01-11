using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Login
{
    public class CreatePasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password Contain : Uppercase, Lowercase, Number, Symbol and minimun length 8")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
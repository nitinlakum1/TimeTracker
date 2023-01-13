using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Remote("ValidateUser", "Login", ErrorMessage = "Couldn’t find your Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Remote("ValidatePassword", "Login", AdditionalFields = "Username", ErrorMessage = "Wrong password. Try again or click Forgot password to reset it.")]
        public string Password { get; set; }
    }
}
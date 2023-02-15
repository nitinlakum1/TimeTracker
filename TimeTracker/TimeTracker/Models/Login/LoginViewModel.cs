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
        public string Password { get; set; }
    }
}
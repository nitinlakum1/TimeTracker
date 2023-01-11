using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.Login
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [Remote("ValidateEmailForgotPass", "Login", ErrorMessage = "Couldn’t find your Email")]
        public string Email { get; set; }
    }
}
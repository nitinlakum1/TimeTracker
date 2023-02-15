using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.User
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should containt between 5 to 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid email.")]
        [Remote("ValidateEmail", "User", AdditionalFields = "Id", ErrorMessage = "Email already exist.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Contact No. is required.")]
        [Remote("ValidateContactNo", "User", AdditionalFields = "Id", ErrorMessage = "Contact No. already exist.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Joining date is required.")]
        public DateTime JoiningDate { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Address { get; set; }

        public DateTime? DOB { get; set; }

        public string? BankName { get; set; }

        public string? AccountNo { get; set; }

        public string? IFSC { get; set; }

        public string? MacAddress { get; set; }

        public bool FromProfile { get; set; } = false;

        public string? Url { get; set; }

        public IFormFile? AvatarFile { get; set; }
    }
}
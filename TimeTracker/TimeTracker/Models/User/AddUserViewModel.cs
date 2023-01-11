﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models.User
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should containt between 5 to 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid email.")]
        [Remote("ValidateEmail", "User", ErrorMessage = "Email already exist.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Contact No. is required.")]
        [RegularExpression (@"^[0-9]{10}$", ErrorMessage = "Please enter a valid Contact No.")]
        [Remote("ValidateContactNo", "User", ErrorMessage = "Contact No. already exist.")]
        public string ContactNo { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Joining date is required.")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Mac Address is required.")]
        public string MacAddress { get; set; }

        public string? Url { get; set; }

        public IFormFile? AvatarFile { get; set; }

        public long? Avatar { get; set; }
    }
}
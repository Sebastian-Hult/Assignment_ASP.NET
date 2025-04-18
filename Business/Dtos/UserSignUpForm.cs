﻿using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserSignUpForm
{
    [Display(Name = "Fisrt Name", Prompt = "Enter first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Required")]
    public string Password { get; set; } = null!;

    [Display(Name = "ConfirmPassword", Prompt = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [Required(ErrorMessage = "Required")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms & Conditions", Prompt = "I accept the terms & conditions.")]
    [Required(ErrorMessage = "Required")]
    public bool TermsAndConditions { get; set; }
}

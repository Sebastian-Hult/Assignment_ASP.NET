using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserSignUpForm
{
    [Display(Name = "Full Name", Prompt = "Enter full name")]
    [DataType(DataType.Text)]
    [Required]
    public string FullName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;

    [Display(Name = "¨ConfirmPassword", Prompt = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
    public bool TermsAndConditions { get; set; }
}

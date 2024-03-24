using System.ComponentModel.DataAnnotations;

namespace NetMVCLearning.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email Address is required")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required!")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords not match!")]
    public string PasswordRepeat { get; set; }
}
using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet.Actions;

namespace NetMVCLearning.ViewModels;

public class LoginViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email Address is required")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
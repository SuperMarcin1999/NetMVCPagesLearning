using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetMVCLearning.Data;
using NetMVCLearning.Models;
using NetMVCLearning.ViewModels;

namespace NetMVCLearning.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ApplicationDbContext dbContext
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }
    
    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        var user = await _userManager.FindByEmailAsync(loginVM.Email);

        if (user is null)
        {
            // TempData is bad practice
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginVM);
        }
        
        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);

        if (!passwordCheck)
        {
            // TempData is bad practice
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginVM);
        }
        
        var signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

        if (signInResult.Succeeded)
        {
            return RedirectToAction("Index", "Race");
        }
        else
        {
            // TempData is bad practice
            TempData["Error"] = "Sign in failed.";
            return View(loginVM);
        }
    }
    
    public IActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM)
    {
        if (!ModelState.IsValid)
            return View(registerVM);

        var user = await _userManager.FindByEmailAsync(registerVM.Email);
        if (user is not null)
        {
            TempData["Error"] = "Email already exist!";
            return View(registerVM);
        }
        
        var newUser = new AppUser()
        {
            Email = registerVM.Email,
            UserName = registerVM.Email
        };
        var creationResult = await _userManager.CreateAsync(newUser, registerVM.Password);
        if (!creationResult.Succeeded)
        {
            TempData["Error"] = creationResult.Errors.FirstOrDefault()?.Description;
            return View(registerVM);
        }
        
        await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        
        return RedirectToAction("Index", "Race");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Race");
    }
}
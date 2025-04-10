using System.Threading.Tasks;
using Business.Dtos;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult SignIn()
    {
        ViewBag.ErrorMessage = "";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(UserLoginForm form, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";

        if (ModelState.IsValid)
        {
            var result = await _authService.LoginAsync(form);
            if (result)
                return Redirect(returnUrl);
        }

        ViewBag.ErrorMessage = "Invalid email or password.";
        return View(form);
    }


    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = "";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserSignUpForm form)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.SignUpAsync(form);
            if (result)
                return LocalRedirect("~/");
        }

        ViewBag.ErrorMessage = "";
        return View(form);
    }
}

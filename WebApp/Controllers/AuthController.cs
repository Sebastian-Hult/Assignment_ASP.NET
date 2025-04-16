using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = "";
        ViewBag.ReturnUrl = returnUrl;

        if (ModelState.IsValid)
        {
            var singInForm = new UserLoginForm
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await _authService.LoginAsync(singInForm);
            if (result)
                return LocalRedirect(returnUrl);
        }

        ViewBag.ErrorMessage = "Invalid email or password.";
        return View(model);
    }


    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = "";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            var signUpForm = new UserSignUpForm
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                TermsAndConditions = model.TermsAndConditions
            };

            var result = await _authService.SignUpAsync(signUpForm);
            if (result)
                return LocalRedirect("~/");
        }

        ViewBag.ErrorMessage = "";
        return View(model);
    }
}

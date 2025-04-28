using System.Security.Claims;
using Business.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<string> GetUserIdAsync(ClaimsPrincipal user);
    Task<bool> LoginAsync(UserLoginForm loginForm);
    Task<bool> SignOutAsync();
    Task<bool> SignUpAsync(UserSignUpForm signupForm);
}

public class AuthService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<bool> LoginAsync(UserLoginForm loginForm)
    {
        var result = await _signInManager.PasswordSignInAsync(loginForm.Email, loginForm.Password, false, false);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> SignUpAsync(UserSignUpForm signupForm)
    {
        var userEntity = new UserEntity
        {
            UserName = signupForm.Email,
            FirstName = signupForm.FirstName,
            LastName = signupForm.LastName,
            Email = signupForm.Email,
        };

        var result = await _userManager.CreateAsync(userEntity, signupForm.Password);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<string> GetUserIdAsync(ClaimsPrincipal user)
    {
        var userEntity = await _userManager.GetUserAsync(user);
        return userEntity?.Id ?? null!;
    }
}

using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Web.Controllers;
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;

    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var (success, error) = await _accountService.LoginAsync(email, password);

        if (success)
            return RedirectToAction("Index", "Home");

        if (!string.IsNullOrEmpty(error)) 
        {
            ModelState.AddModelError("", error);
        }
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register(string fullName, string email, string password)
    {
        var (success, error) = await _accountService.RegisterAsync(fullName, email, password);

        if (success)
            return RedirectToAction("Login");

        if (!string.IsNullOrEmpty(error)) 
        {
            ModelState.AddModelError("", error);
        }
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}



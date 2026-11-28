using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Services;

namespace ResidentEvil.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // REGISTER
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required!");
                return View();
            }

            var result = await _authService.RegisterAsync(username, password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            // we show the errors in the view if registration fails
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        //  LOGIN
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "All fields are required!");
                return View();
            }

            var result = await _authService.LoginAsync(username, password, rememberMe);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Access Denied: Invalid Username or Password!");
            return View();
        }

        // LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken] // protection
        public async Task<IActionResult> Logout()
        {
            // this deletes cookies and signs out the user
            await _authService.LogoutAsync();

            // redirect to home page after logout
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }

}

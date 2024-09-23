using Library_MVCProject.Models;
using Library_MVCProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library_MVCProject.Controllers
{
    public class AuthController : Controller
    {
        static List<User> _users = new List<User>()             // A static list to store users
        {
            new User
            {
                Id = 1,
                FullName = "Atakan Çelik",
                Email    = "atakan@patika.dev",
                Password = "CfDJ8JmRK1JDn7xGqb6jE6i_gIuBT8QODLmx5K2dc9V1BhkielTSTH4Rttxb6gumtbKlCSIsER2s0feB-QKk2Hr6se9a7P57v28InFfNjoc0TuC-yeHHvLHBitttJgU8lOLXsQ",         // To change this static data enter '123' in the password
                PhoneNumber = "1234567890",
            }
        };

        private readonly IDataProtector _dataProtector;
        // Constructor to create the data protector for encryption and decryption
        public AuthController(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("Security");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            if (@User.Claims.FirstOrDefault(x => x.Type == "id")?.Value != null)
            {
                return RedirectToAction("List", "Book");
            }

            return View();
        }
        [HttpPost]
        public IActionResult SignUp(RegisterViewModel formData)
        { // Check if the form data is valid
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            // Check if the user already exists by email
            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());
            if (user != null)
            {
                ViewBag.Error = "User available";
                return View(formData);
            }
            // Create a new user with encrypted password
            var newUser = new User()
            {
                Id = _users.Max(x => x.Id) + 1,
                FullName = formData.FullName,
                Email = formData.Email.ToLower(),
                Password = _dataProtector.Protect(formData.Password),
                PhoneNumber = formData.PhoneNumber,

            };

            _users.Add(newUser); // Add new user to the static list

            return RedirectToAction("SignIn", "Auth"); // Redirect to the SignIn page after successful sign-up
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (@User.Claims.FirstOrDefault(x => x.Type == "id")?.Value != null) // Redirect to book list if the user is already logged in
            {
                return RedirectToAction("List", "Book");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel formData)
        {
            if (!ModelState.IsValid)   // Check if the form data is valid
            {
                ViewBag.Error = "Please fill in all fields correctly.";
                return View(formData);
            }
            // Find user by email
            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());
            if (user is null)
            {

                ViewBag.Error = "Username or password is incorrect";
                return View(formData);
            }
            // Decrypt password to check the raw value
            var rawPassword = _dataProtector.Unprotect(user.Password);
            // Validate the password
            if (rawPassword == formData.Password)
            {

                var claims = new List<Claim>();
                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("id", user.Id.ToString()));
                claims.Add(new Claim("fullname", user.FullName));
                claims.Add(new Claim("phonenumber", user.PhoneNumber));
                claims.Add(new Claim("joindate", user.JoinDate.ToShortDateString()));

                // Create a claim identity and authentication properties for cookies
                var claimIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var autProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(10))
                };
                // Sign in the user using cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIndentity), autProperties);
            }
            else
            {
                ViewBag.Error = "Username or password is incorrect";
                return View(formData);
            }
            // Redirect to book list after successful sign-in
            return RedirectToAction("List", "Book");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(); // Sign out the user by clearing cookies
            return RedirectToAction("Index", "Home");
        }
    }
}

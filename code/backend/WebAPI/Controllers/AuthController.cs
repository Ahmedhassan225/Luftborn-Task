using Application.UnitOfWork.Interfaces;
using Domain.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Controllers._Base;

namespace WebAPI.Controllers
{
    public class AuthController : BaseApiController
    {
        public AuthController()
        {
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
            }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("login-callback")]
        public async Task<IActionResult> LoginCallback(string returnUrl = "/")
        {
            // Get the user information
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Authentication failed");

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var userInfo = new
            {
                Email = email,
                Name = name,
                IsAuthenticated = true
            };

            // In a real-world app, generate a JWT here .. A.Hassan
            return Redirect($"{returnUrl}?auth={System.Text.Json.JsonSerializer.Serialize(userInfo)}");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}

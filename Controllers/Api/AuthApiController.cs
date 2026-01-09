using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using E_Gostinc.Models;
using System.Threading.Tasks;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthApiController : ControllerBase
    {
        private readonly UserManager<Uporabnik> _userManager;
        private readonly SignInManager<Uporabnik> _signInManager;

        public AuthApiController(UserManager<Uporabnik> userManager, SignInManager<Uporabnik> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Email in geslo sta obvezna" });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { error = "Napačno uporabniško ime ali geslo" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { error = "Napačno uporabniško ime ali geslo" });
            }

            // Get user role
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                delovno_mesto = user.Delovno_mesto,
                role = role,
                message = "Prijava uspešna"
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Odjava uspešna" });
        }

        [HttpGet("check")]
        public async Task<ActionResult> CheckAuth()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(new
                    {
                        authenticated = true,
                        email = user.Email,
                        role = roles.FirstOrDefault()
                    });
                }
            }
            return Ok(new { authenticated = false });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
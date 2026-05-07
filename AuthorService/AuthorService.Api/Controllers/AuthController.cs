using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication.Services;

namespace AuthorService.Api.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("token")]
        public IActionResult Authenticate([FromBody] LoginRequest loginRequest)
        {
            // Replace with actual user validation logic
            if (loginRequest.Username != "testuser" || loginRequest.Password != "password")
                return Unauthorized();

            var token = _tokenService.GenerateToken(loginRequest.Username, "Admin");
            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
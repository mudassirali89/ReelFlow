using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using ReelFLow.DTO;

namespace ReelFLow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReelFlowAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        IConfiguration configuration;
        public ReelFlowAuthController(UserManager<IdentityUser> _userManager, IConfiguration configuration)
        {
            this._userManager = _userManager;
            this.configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registerDTO)
        {
            var usernameExists = await _userManager.FindByNameAsync(registerDTO.UserName);
            if (usernameExists != null)
            {
                return Conflict(new { message = "UserName already exists" });
            }
            var emailExists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (emailExists != null)
            {
                return Conflict(new { message = "Email already exists" });
            }
            IdentityUser newUser = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "User creation failed", errors });
            }
            return Ok(new { message = "user created successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            IdentityUser user = await _userManager.FindByNameAsync(loginDTO.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDTO.UsernameOrEmail);
            }

            if (user == null)
            {
                return NotFound("Username or Email does not exist");
            }

            if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                var jwtToken = GenerateJWtToken(user);
                return new JsonResult(new { token = jwtToken,usernameOrEmail=loginDTO.UsernameOrEmail });
            }

            return BadRequest("Incorrect Password");
        }

        private string GenerateJWtToken(IdentityUser user)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenExpiry = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:Expiry"]));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: tokenExpiry,
                signingCredentials: creds,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

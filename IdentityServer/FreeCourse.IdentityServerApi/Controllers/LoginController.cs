using FreeCourse.IdentityServerApi.Data;
using FreeCourse.IdentityServerApi.Dtos;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace FreeCourse.IdentityServerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Index(LoginDto loginDto)
    {
        if (loginDto.UserName == null) return BadRequest();
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user == null) { return BadRequest(Response<NoContent>.Fail("User Name is not found", 404)); }
        if (loginDto.Password == null) return BadRequest();
        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        return result.Succeeded ? Ok(new { token = GenerateJwtToken(user) }) : BadRequest();
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var value = configuration.GetSection("AppSetting:Secret").Value;
        if (value == null) return "";
        var key = Encoding.ASCII.GetBytes(value);
        if (user.UserName == null) return "";
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[]
            {
                // new(ClaimTypes.NameIdentifier,user.Id),
                // new(ClaimTypes.Name,user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Name,user.UserName)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
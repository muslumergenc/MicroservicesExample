using System.IdentityModel.Tokens.Jwt;
using FreeCourse.IdentityServerApi.Data;
using FreeCourse.IdentityServerApi.Dtos;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.IdentityServerApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        var user = new ApplicationUser {
            UserName = signUpDto.UserName,
            Email = signUpDto.Email,
            City = signUpDto.City
        };
        if (signUpDto.Password == null) return BadRequest();
        var result = await userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded)
        { return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400)); }
        return result.Succeeded ? Ok() : BadRequest();
    }

    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser()
    {
        var userIdClaim =User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name);
        if (userIdClaim is null) return BadRequest();
        var user = await userManager.FindByNameAsync(userIdClaim.Value);
        if (user is null) return BadRequest();
        return Ok(new { user.Id, user.UserName, user.Email, user.City });
    }
}
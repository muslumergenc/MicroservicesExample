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
}
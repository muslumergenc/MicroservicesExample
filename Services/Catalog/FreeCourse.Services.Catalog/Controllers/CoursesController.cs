using FreeCourse.Services.Catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    // public async Task<IActionResult> GetById(string id)
    // {
    //     var response = await _courseService.GetByIdAsync(id);
    //     
    // }
}
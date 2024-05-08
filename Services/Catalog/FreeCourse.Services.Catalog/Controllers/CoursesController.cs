using FreeCourse.Services.Catalog.Dtos.CourseDtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : CustomBaseController
{  [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await courseService.GetAllAsync();
        return CreateActionResultInstance(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await courseService.GetByIdAsync(id);
        return CreateActionResultInstance(response);
    }
    [Route("/api/[controller]/GetAllByUserId/{userid}")]
    [HttpGet]
    public async Task<IActionResult> GetByUserId(string userid)
    {
        var response = await courseService.GetByIdAsync(userid);
        return CreateActionResultInstance(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        var response = await courseService.CreateAsync(dto);
        return CreateActionResultInstance(response);
    }
    [HttpPut]
    public async Task<IActionResult> Update(CourseUpdateDto dto)
    {
        var response = await courseService.UpdateAsync(dto);
        return CreateActionResultInstance(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await courseService.DeleteAsync(id);
        return CreateActionResultInstance(response);
    }
}
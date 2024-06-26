﻿using FreeCourse.Services.Catalog.Dtos.CategoryDtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : CustomBaseController
{
    private readonly ICategoryService _categoryService = categoryService;
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _categoryService.GetAllAsync();
        return CreateActionResultInstance(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _categoryService.GetByIdAsync(id);
        return CreateActionResultInstance(response);
    }
  
    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        var response = await _categoryService.CreateAsync(dto);
        return CreateActionResultInstance(response);
    }
}
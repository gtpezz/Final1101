using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с категориями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Получить категорию по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new { message = "Категория не найдена" });
        return Ok(category);
    }

    /// <summary>
    /// Добавить категорию
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _categoryService.CategoryExistsAsync(category.Name))
            return BadRequest(new { message = "Категория с таким названием уже существует" });

        await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    /// <summary>
    /// Обновить категорию
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.Id)
            return BadRequest(new { message = "ID не совпадают" });

        var existing = await _categoryService.GetCategoryByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = "Категория не найдена" });

        await _categoryService.UpdateCategoryAsync(category);
        return Ok(category);
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new { message = "Категория не найдена" });

        await _categoryService.DeleteCategoryAsync(id);
        return Ok(new { message = "Категория удалена" });
    }
}
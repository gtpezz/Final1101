using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Controllers;


/// <summary>
/// Контроллер для работы с товарами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductService service) : ControllerBase
{
    private readonly ProductService _service = service;

    /// <summary>
    /// Получить все товары
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Получить товар по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Товар не найден" });
        return Ok(product);
    }

    /// <summary>
    /// Получить товар по артикулу
    /// </summary>
    [HttpGet("article/{article}")]
    public async Task<IActionResult> GetByArticle(string article)
    {
        var product = await _service.GetProductByArticleAsync(article);
        if (product == null)
            return NotFound(new { message = "Товар не найден" });
        return Ok(product);
    }

    /// <summary>
    /// Поиск товаров по названию
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var products = await _service.SearchProductsAsync(term);
        return Ok(products);
    }

    /// <summary>
    /// Фильтрация товаров
    /// </summary>
    [HttpGet("filter")]
    public async Task<IActionResult> Filter(
        [FromQuery] int? manufacturerId,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var products = await _service.FilterProductsAsync(
            manufacturerId, categoryId, minPrice, maxPrice);
        return Ok(products);
    }

    /// <summary>
    /// Получить товары по категории
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var products = await _service.GetProductsByCategoryAsync(categoryId);
        return Ok(products);
    }

    /// <summary>
    /// Получить товары по производителю
    /// </summary>
    [HttpGet("manufacturer/{manufacturerId}")]
    public async Task<IActionResult> GetByManufacturer(int manufacturerId)
    {
        var products = await _service.GetProductsByManufacturerAsync(manufacturerId);
        return Ok(products);
    }

    /// <summary>
    /// Получить количество товаров
    /// </summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        var count = await _service.GetTotalCountAsync();
        return Ok(new { total = count });
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Обновить товар
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id)
            return BadRequest(new { message = "ID не совпадают" });

        var existing = await _service.GetProductByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = "Товар не найден" });

        await _service.UpdateProductAsync(product);
        return Ok(product);
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Товар не найден" });

        await _service.DeleteProductAsync(id);
        return Ok(new { message = "Товар удален" });
    }
}
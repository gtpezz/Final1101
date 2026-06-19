using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с производителями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ManufacturersController : ControllerBase
{
    private readonly ManufacturerService _manufacturerService;

    public ManufacturersController(ManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    /// <summary>
    /// Получить всех производителей
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
        return Ok(manufacturers);
    }

    /// <summary>
    /// Получить производителя по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        if (manufacturer == null)
            return NotFound(new { message = "Производитель не найден" });
        return Ok(manufacturer);
    }

    /// <summary>
    /// Добавить производителя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Manufacturer manufacturer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _manufacturerService.ManufacturerExistsAsync(manufacturer.Name))
            return BadRequest(new { message = "Производитель с таким названием уже существует" });

        await _manufacturerService.AddManufacturerAsync(manufacturer);
        return CreatedAtAction(nameof(GetById), new { id = manufacturer.Id }, manufacturer);
    }

    /// <summary>
    /// Обновить производителя
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Manufacturer manufacturer)
    {
        if (id != manufacturer.Id)
            return BadRequest(new { message = "ID не совпадают" });

        var existing = await _manufacturerService.GetManufacturerByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = "Производитель не найден" });

        await _manufacturerService.UpdateManufacturerAsync(manufacturer);
        return Ok(manufacturer);
    }

    /// <summary>
    /// Удалить производителя
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        if (manufacturer == null)
            return NotFound(new { message = "Производитель не найден" });

        await _manufacturerService.DeleteManufacturerAsync(id);
        return Ok(new { message = "Производитель удален" });
    }
}

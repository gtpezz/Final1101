using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Controllers;

/// <summary>
/// Контроллер для работы со статусами заказов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderStatusesController : ControllerBase
{
    private readonly OrderStatusService _orderStatusService;

    public OrderStatusesController(OrderStatusService orderStatusService)
    {
        _orderStatusService = orderStatusService;
    }

    /// <summary>
    /// Получить все статусы заказов
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var statuses = await _orderStatusService.GetAllStatusesAsync();
        return Ok(statuses);
    }

    /// <summary>
    /// Получить статус по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var status = await _orderStatusService.GetStatusByIdAsync(id);
        if (status == null)
            return NotFound(new { message = "Статус не найден" });
        return Ok(status);
    }

    /// <summary>
    /// Получить статус по названию
    /// </summary>
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var status = await _orderStatusService.GetStatusByNameAsync(name);
        if (status == null)
            return NotFound(new { message = "Статус не найден" });
        return Ok(status);
    }

    /// <summary>
    /// Добавить статус
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderStatus status)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _orderStatusService.AddStatusAsync(status);
        return CreatedAtAction(nameof(GetById), new { id = status.Id }, status);
    }

    /// <summary>
    /// Обновить статус
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderStatus status)
    {
        if (id != status.Id)
            return BadRequest(new { message = "ID не совпадают" });

        var existing = await _orderStatusService.GetStatusByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = "Статус не найден" });

        await _orderStatusService.UpdateStatusAsync(status);
        return Ok(status);
    }

    /// <summary>
    /// Удалить статус
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await _orderStatusService.GetStatusByIdAsync(id);
        if (status == null)
            return NotFound(new { message = "Статус не найден" });

        await _orderStatusService.DeleteStatusAsync(id);
        return Ok(new { message = "Статус удален" });
    }
}
using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с пользователями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService service) : ControllerBase
{
    private readonly UserService _service = service;

    
    
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });
        return Ok(user);
    }

    /// <summary>
    /// Получить пользователя по логину
    /// </summary>
    [HttpGet("login/{login}")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        var user = await _service.GetUserByLoginAsync(login);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });
        return Ok(user);
    }

    /// <summary>
    /// Получить пользователей по роли
    /// </summary>
    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetByRole(int roleId)
    {
        var users = await _service.GetUsersByRoleAsync(roleId);
        return Ok(users);
    }

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _service.UserExistsAsync(user.Login))
            return BadRequest(new { message = "Пользователь с таким логином уже существует" });

        await _service.AddUserAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest(new { message = "ID не совпадают" });

        var existing = await _service.GetUserByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = "Пользователь не найден" });

        await _service.UpdateUserAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });

        await _service.DeleteUserAsync(id);
        return Ok(new { message = "Пользователь удален" });
    }

    /// <summary>
    /// Получить количество пользователей
    /// </summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        var count = await _service.GetUsersCountAsync();
        return Ok(new { total = count });
    }
}

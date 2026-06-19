using Microsoft.AspNetCore.Mvc;
using TradeTerminal.Application.Services;
using TradeTerminal.WebApi.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Controllers;

/// <summary>
/// Контроллер для авторизации
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            return BadRequest(new { message = "Логин и пароль обязательны" });

        var user = await _userService.AuthenticateUserAsync(request.Login, request.Password);

        if (user == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Login,
            user.RoleId,
            RoleName = user.RoleName,  // ← Исправлено: используем RoleName, а не Role
            message = "Вход выполнен успешно"
        });
    }

    /// <summary>
    /// Проверка существования пользователя
    /// </summary>
    [HttpPost("check")]
    public async Task<IActionResult> CheckUser([FromBody] LoginRequest request)
    {
        var user = await _userService.GetUserByLoginAsync(request.Login);
        if (user == null)
            return NotFound(new { exists = false, message = "Пользователь не найден" });

        return Ok(new { exists = true, userId = user.Id, fullName = user.FullName });
    }

    /// <summary>
    /// Получить текущего пользователя
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser([FromQuery] string login)
    {
        var user = await _userService.GetUserByLoginAsync(login);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Login,
            user.RoleId,
            RoleName = user.RoleName  // ← Исправлено: используем RoleName, а не Role
        });
    }
}
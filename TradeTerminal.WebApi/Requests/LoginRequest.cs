// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeTerminal.WebApi.Requests;

/// <summary>
/// Запрос на вход
/// </summary>
public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}
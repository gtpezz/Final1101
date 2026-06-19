namespace TradeTerminal.WebApi.Requests;

/// <summary>
/// Запрос на создание заказа
/// </summary>
public class CreateOrderRequest
{
    public int? UserId { get; set; }
}

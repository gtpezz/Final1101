namespace TradeTerminal.WebApi.Requests;

/// <summary>
/// Запрос на добавление товара в заказ
/// </summary>
public class AddItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

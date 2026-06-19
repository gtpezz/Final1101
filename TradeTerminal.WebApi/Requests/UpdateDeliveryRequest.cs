namespace TradeTerminal.WebApi.Requests;

/// <summary>
/// Запрос на обновление даты доставки
/// </summary>
public class UpdateDeliveryRequest
{
    public DateTime DeliveryDate { get; set; }
}

namespace TradeTerminal.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public int? UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }
    public string PickupCode { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

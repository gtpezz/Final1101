namespace TradeTerminal.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public int? UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int StatusId { get; set; }
    public string PickupCode { get; set; }
    public decimal TotalAmount { get; set; }

    public User User { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}

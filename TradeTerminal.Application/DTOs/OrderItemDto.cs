namespace TradeTerminal.Application.DTOs;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductArticle { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtOrder { get; set; }
    public decimal DiscountAtOrder { get; set; }
    public decimal Total => Quantity * PriceAtOrder * (1 - DiscountAtOrder / 100);
}

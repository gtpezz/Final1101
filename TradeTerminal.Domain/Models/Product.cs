namespace TradeTerminal.Domain.Models;

public class Product
{
    public int Id { get; set; }

    public string Article { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public string? Author { get; set; }

    public int? ManufacturerId { get; set; }
    public int? CategoryId { get; set; }

    public decimal Discount { get; set; }
    public int StockQuantity { get; set; }

    public string? Description { get; set; }
    public string? Photo { get; set; }

    public Manufacturer? Manufacturer { get; set; }
    public Category? Category { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

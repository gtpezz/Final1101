namespace TradeTerminal.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Article { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public string? Author { get; set; }

    public int? ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; } 

    public decimal Discount { get; set; }
    public int StockQuantity { get; set; }

    public string? Description { get; set; }
    public string? Photo { get; set; }

    public decimal PriceWithDiscount => Price * (1 - Discount / 100);
}

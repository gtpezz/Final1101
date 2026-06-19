namespace TradeTerminal.Web.Pages;

public partial class IndexModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string? ManufacturerName { get; set; }
        public string? Photo { get; set; }
    }
}
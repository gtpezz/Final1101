using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeTerminal.Web.Pages;

public class MessageModel : PageModel
{
    public string? ProductName { get; set; }
    public decimal ProductPrice { get; set; }

    public void OnGet(string? productName, decimal productPrice)
    {
        ProductName = productName ?? "Товар";
        ProductPrice = productPrice;
    }
}


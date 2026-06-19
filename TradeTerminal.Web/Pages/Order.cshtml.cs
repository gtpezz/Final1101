using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace TradeTerminal.Web.Pages;

public class OrderModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public OrderModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnPostAddItemAsync(int productId, string productName, decimal productPrice)
    {
        try
        {
            var client = _clientFactory.CreateClient("ApiClient");

            var orderId = HttpContext.Session.GetInt32("OrderId");

            if (!orderId.HasValue)
            {
                var createResponse = await client.PostAsync("/api/orders",
                    new StringContent("{}", Encoding.UTF8, "application/json"));

                if (createResponse.IsSuccessStatusCode)
                {
                    var json = await createResponse.Content.ReadAsStringAsync();
                    var order = JsonSerializer.Deserialize<JsonElement>(json);
                    orderId = order.GetProperty("id").GetInt32();
                    HttpContext.Session.SetInt32("OrderId", orderId.Value);
                }
            }

            if (orderId.HasValue)
            {
                var request = new { productId, quantity = 1 };
                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                await client.PostAsync($"/api/orders/{orderId}/items", content);
            }

            return RedirectToPage("/Message", new { productName, productPrice });
        }
        catch
        {
            return RedirectToPage("/Message", new { productName, productPrice });
        }
    }
}
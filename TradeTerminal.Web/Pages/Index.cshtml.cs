using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace TradeTerminal.Web.Pages;

public partial class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public List<Product> Products { get; set; } = new();

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task OnGetAsync()
    {
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/products");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<JsonElement>(json);

                Products.Clear();
                foreach (var item in products.EnumerateArray())
                {
                    var product = new Product
                    {
                        Id = item.GetProperty("id").GetInt32(),
                        Name = item.GetProperty("name").GetString() ?? "Без названия",
                        Price = item.GetProperty("price").GetDecimal(),
                        ManufacturerName = item.TryGetProperty("manufacturerName", out var m)
                            ? m.GetString() ?? "Не указан"
                            : "Не указан",
                        Photo = item.TryGetProperty("photo", out var photo)
                            ? photo.GetString()
                            : null
                    };
                    Products.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
        }
    }
}
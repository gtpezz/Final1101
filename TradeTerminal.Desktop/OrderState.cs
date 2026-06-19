using System.Text.Json;

namespace TradeTerminal.Desktop;

/// <summary>
/// Состояние текущего заказа (Singleton)
/// </summary>
public class OrderState
{
    private static OrderState? _instance;
    private static readonly object _lock = new();

    public JsonElement? CurrentOrder { get; private set; }
    public event EventHandler? OrderChanged;

    private OrderState() { }

    public static OrderState Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new OrderState();
                }
            }
            return _instance;
        }
    }

    public bool HasItems
    {
        get
        {
            if (!CurrentOrder.HasValue) return false;
            var items = CurrentOrder.Value.GetProperty("items");
            return items.GetArrayLength() > 0;
        }
    }

    public int ItemsCount
    {
        get
        {
            if (!CurrentOrder.HasValue) return 0;
            var items = CurrentOrder.Value.GetProperty("items");
            int count = 0;
            foreach (var item in items.EnumerateArray())
            {
                count += item.GetProperty("quantity").GetInt32();
            }
            return count;
        }
    }

    public decimal TotalAmount
    {
        get
        {
            if (!CurrentOrder.HasValue) return 0;
            return CurrentOrder.Value.GetProperty("totalAmount").GetDecimal();
        }
    }

    public int OrderId
    {
        get
        {
            if (!CurrentOrder.HasValue) return 0;
            return CurrentOrder.Value.GetProperty("id").GetInt32();
        }
    }

    public int OrderNumber
    {
        get
        {
            if (!CurrentOrder.HasValue) return 0;
            return CurrentOrder.Value.GetProperty("orderNumber").GetInt32();
        }
    }

    public string? PickupCode
    {
        get
        {
            if (!CurrentOrder.HasValue) return null;
            return CurrentOrder.Value.GetProperty("pickupCode").GetString();
        }
    }

    public string? UserFullName
    {
        get
        {
            if (!CurrentOrder.HasValue) return null;
            return CurrentOrder.Value.TryGetProperty("userFullName", out var prop)
                ? prop.GetString()
                : null;
        }
    }

    public void SetOrder(JsonElement? order)
    {
        CurrentOrder = order;
        OrderChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearOrder()
    {
        CurrentOrder = null;
        OrderChanged?.Invoke(this, EventArgs.Empty);
    }
}
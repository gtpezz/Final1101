using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services
{
    public interface IOrderItemService
    {
        Task DeleteOrderItemAsync(int id);
        Task<OrderItem?> GetOrderItemByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByProductIdAsync(int productId);
        Task UpdateOrderItemQuantityAsync(int orderItemId, int quantity);
    }
}
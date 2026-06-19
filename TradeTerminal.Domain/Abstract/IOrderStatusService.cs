using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services
{
    public interface IOrderStatusService
    {
        Task AddStatusAsync(OrderStatus status);
        Task DeleteStatusAsync(int id);
        Task<IEnumerable<OrderStatus>> GetAllStatusesAsync();
        Task<OrderStatus?> GetStatusByIdAsync(int id);
        Task<OrderStatus?> GetStatusByNameAsync(string name);
        Task UpdateStatusAsync(OrderStatus status);
    }
}
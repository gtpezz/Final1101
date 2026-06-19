using Microsoft.EntityFrameworkCore;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы со статусами заказов
/// </summary>
public class OrderStatusService : IOrderStatusService
{
    private readonly DatabaseContext _context;

    public OrderStatusService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить все статусы заказов
    /// </summary>
    public async Task<IEnumerable<OrderStatus>> GetAllStatusesAsync()
    {
        return await _context.OrderStatuses
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    /// <summary>
    /// Получить статус по идентификатору
    /// </summary>
    public async Task<OrderStatus?> GetStatusByIdAsync(int id)
    {
        return await _context.OrderStatuses
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Получить статус по названию
    /// </summary>
    public async Task<OrderStatus?> GetStatusByNameAsync(string name)
    {
        return await _context.OrderStatuses
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    /// <summary>
    /// Добавить статус
    /// </summary>
    public async Task AddStatusAsync(OrderStatus status)
    {
        _context.OrderStatuses.Add(status);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить статус
    /// </summary>
    public async Task UpdateStatusAsync(OrderStatus status)
    {
        _context.OrderStatuses.Update(status);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить статус
    /// </summary>
    public async Task DeleteStatusAsync(int id)
    {
        var status = await _context.OrderStatuses.FindAsync(id);
        if (status != null)
        {
            _context.OrderStatuses.Remove(status);
            await _context.SaveChangesAsync();
        }
    }
}

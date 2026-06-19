using Microsoft.EntityFrameworkCore;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы с позициями заказов
/// </summary>
public class OrderItemService : IOrderItemService
{
    private readonly DatabaseContext _context;

    public OrderItemService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить позицию заказа по идентификатору
    /// </summary>
    public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
    {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .FirstOrDefaultAsync(oi => oi.Id == id);
    }

    /// <summary>
    /// Получить все позиции заказа
    /// </summary>
    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
    }

    /// <summary>
    /// Получить все позиции с товаром
    /// </summary>
    public async Task<IEnumerable<OrderItem>> GetOrderItemsByProductIdAsync(int productId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.ProductId == productId)
            .ToListAsync();
    }

    /// <summary>
    /// Обновить количество позиции
    /// </summary>
    public async Task UpdateOrderItemQuantityAsync(int orderItemId, int quantity)
    {
        var item = await _context.OrderItems.FindAsync(orderItemId);
        if (item != null)
        {
            item.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Удалить позицию из заказа
    /// </summary>
    public async Task DeleteOrderItemAsync(int id)
    {
        var item = await _context.OrderItems.FindAsync(id);
        if (item != null)
        {
            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
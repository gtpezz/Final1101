using Microsoft.EntityFrameworkCore;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы с производителями
/// </summary>
public class ManufacturerService : IManufacturerService
{
    private readonly DatabaseContext _context;

    public ManufacturerService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить всех производителей
    /// </summary>
    public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
    {
        return await _context.Manufacturers
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Получить производителя по идентификатору
    /// </summary>
    public async Task<Manufacturer?> GetManufacturerByIdAsync(int id)
    {
        return await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Получить производителя по названию
    /// </summary>
    public async Task<Manufacturer?> GetManufacturerByNameAsync(string name)
    {
        return await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == name);
    }

    /// <summary>
    /// Добавить производителя
    /// </summary>
    public async Task AddManufacturerAsync(Manufacturer manufacturer)
    {
        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить производителя
    /// </summary>
    public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
    {
        _context.Manufacturers.Update(manufacturer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить производителя
    /// </summary>
    public async Task DeleteManufacturerAsync(int id)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id);
        if (manufacturer != null)
        {
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Проверить существует ли производитель с таким названием
    /// </summary>
    public async Task<bool> ManufacturerExistsAsync(string name)
    {
        return await _context.Manufacturers.AnyAsync(m => m.Name == name);
    }
}

using Microsoft.EntityFrameworkCore;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы с ролями
/// </summary>
public class RoleService : IRoleService
{
    private readonly DatabaseContext _context;

    public RoleService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить все роли
    /// </summary>
    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles
            .OrderBy(r => r.Id)
            .ToListAsync();
    }

    /// <summary>
    /// Получить роль по идентификатору
    /// </summary>
    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получить роль по названию
    /// </summary>
    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    /// <summary>
    /// Добавить роль
    /// </summary>
    public async Task AddRoleAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить роль
    /// </summary>
    public async Task UpdateRoleAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить роль
    /// </summary>
    public async Task DeleteRoleAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TradeTerminal.Application.DTOs;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

public class UserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить всех пользователей в виде DTO
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.Name : null,
                FullName = u.FullName,
                Login = u.Login
            })
            .ToListAsync();
    }

    /// <summary>
    /// Получить пользователя по ID в виде DTO
    /// </summary>
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.Name : null,
                FullName = u.FullName,
                Login = u.Login
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получить пользователя по логину в виде DTO
    /// </summary>
    public async Task<UserDto?> GetUserByLoginAsync(string login)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Login == login)
            .Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.Name : null,
                FullName = u.FullName,
                Login = u.Login
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получить пользователей по роли в виде DTO
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.Name : null,
                FullName = u.FullName,
                Login = u.Login
            })
            .ToListAsync();
    }

    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    public async Task<UserDto?> AuthenticateUserAsync(string login, string password)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Login == login && u.Password == password)
            .Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.Name : null,
                FullName = u.FullName,
                Login = u.Login
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Проверить существует ли пользователь с таким логином
    /// </summary>
    public async Task<bool> UserExistsAsync(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    /// <summary>
    /// Получить количество пользователей
    /// </summary>
    public async Task<int> GetUsersCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    /// <summary>
    /// Получить сущность пользователя (для внутреннего использования)
    /// </summary>
    public async Task<User?> GetUserEntityByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Получить сущность пользователя по логину (для внутреннего использования)
    /// </summary>
    public async Task<User> GetUserEntityByLoginAsync(string login)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == login);
    }
}
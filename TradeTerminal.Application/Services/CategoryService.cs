using Microsoft.EntityFrameworkCore;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы с категориями
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly DatabaseContext _context;

    public CategoryService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Получить категорию по идентификатору
    /// </summary>
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Получить категорию по названию
    /// </summary>
    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    /// <summary>
    /// Добавить категорию
    /// </summary>
    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить категорию
    /// </summary>
    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Проверить существует ли категория с таким названием
    /// </summary>
    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await _context.Categories.AnyAsync(c => c.Name == name);
    }
}
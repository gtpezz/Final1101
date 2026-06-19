using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(string name);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task UpdateCategoryAsync(Category category);
    }
}
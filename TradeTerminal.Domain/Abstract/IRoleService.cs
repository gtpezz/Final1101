using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services
{
    public interface IRoleService
    {
        Task AddRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role?> GetRoleByNameAsync(string name);
        Task UpdateRoleAsync(Role role);
    }
}
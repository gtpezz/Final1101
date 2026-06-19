using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services
{
    public interface IManufacturerService
    {
        Task AddManufacturerAsync(Manufacturer manufacturer);
        Task DeleteManufacturerAsync(int id);
        Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
        Task<Manufacturer?> GetManufacturerByIdAsync(int id);
        Task<Manufacturer?> GetManufacturerByNameAsync(string name);
        Task<bool> ManufacturerExistsAsync(string name);
        Task UpdateManufacturerAsync(Manufacturer manufacturer);
    }
}
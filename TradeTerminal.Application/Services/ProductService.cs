using Microsoft.EntityFrameworkCore;
using TradeTerminal.Application.DTOs;
using TradeTerminal.DataAccess;
using TradeTerminal.Domain.Models;

namespace TradeTerminal.Application.Services;

/// <summary>
/// Сервис для работы с товарами
/// </summary>
public class ProductService
{
    private readonly DatabaseContext _context;

    public ProductService(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить все товары в виде DTO
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.StockQuantity > 0)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .ToListAsync();
    }

    /// <summary>
    /// Получить товар по ID в виде DTO
    /// </summary>
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получить товар по артикулу в виде DTO
    /// </summary>
    public async Task<ProductDto?> GetProductByArticleAsync(string article)
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.Article == article)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получить товары по категории в виде DTO
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.StockQuantity > 0)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .ToListAsync();
    }

    /// <summary>
    /// Получить товары по производителю в виде DTO
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetProductsByManufacturerAsync(int manufacturerId)
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.ManufacturerId == manufacturerId && p.StockQuantity > 0)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .ToListAsync();
    }

    /// <summary>
    /// Поиск товаров по названию в виде DTO
    /// </summary>
    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllProductsAsync();

        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) &&
                        p.StockQuantity > 0)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Article = p.Article,
                Name = p.Name,
                Unit = p.Unit,
                Price = p.Price,
                Author = p.Author,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Photo = p.Photo
            })
            .ToListAsync();
    }

    /// <summary>
    /// Фильтрация товаров в виде DTO
    /// </summary>
    public async Task<IEnumerable<ProductDto>> FilterProductsAsync(
        int? manufacturerId,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var query = _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .Where(p => p.StockQuantity > 0)
            .AsQueryable();

        if (manufacturerId.HasValue)
            query = query.Where(p => p.ManufacturerId == manufacturerId);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        return await query.Select(p => new ProductDto
        {
            Id = p.Id,
            Article = p.Article,
            Name = p.Name,
            Unit = p.Unit,
            Price = p.Price,
            Author = p.Author,
            ManufacturerId = p.ManufacturerId,
            ManufacturerName = p.Manufacturer != null ? p.Manufacturer.Name : null,
            CategoryId = p.CategoryId,
            CategoryName = p.Category != null ? p.Category.Name : null,
            Discount = p.Discount,
            StockQuantity = p.StockQuantity,
            Description = p.Description,
            Photo = p.Photo
        }).ToListAsync();
    }

    /// <summary>
    /// Сортировка товаров
    /// </summary>
    public IEnumerable<ProductDto> SortProducts(
        IEnumerable<ProductDto> products,
        string sortBy,
        bool ascending)
    {
        if (string.IsNullOrEmpty(sortBy))
            return products;

        if (sortBy.ToLower() == "name")
        {
            return ascending
                ? products.OrderBy(p => p.Name)
                : products.OrderByDescending(p => p.Name);
        }
        else if (sortBy.ToLower() == "price")
        {
            return ascending
                ? products.OrderBy(p => p.Price)
                : products.OrderByDescending(p => p.Price);
        }

        return products;
    }

    /// <summary>
    /// Получить количество товаров
    /// </summary>
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Products
            .Where(p => p.StockQuantity > 0)
            .CountAsync();
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить товар
    /// </summary>
    public async Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
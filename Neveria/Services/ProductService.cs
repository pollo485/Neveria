using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public class ProductService : IProductService
    {
        private readonly DbFreezeDreamContext _context;
        public ProductService(DbFreezeDreamContext context) => _context = context;

        public async Task<List<ProductoDetalleDTO>> GetAllActiveAsync() =>
            await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.TagCategorieNavigation)
                .Include(p => p.Inventory)
                .Select(p => new ProductoDetalleDTO
                {
                    TagProduct = p.TagProduct,
                    NameProduct = p.NameProduct,
                    UnitPrice = p.UnitPrice,
                    DescriptionProduct = p.DescriptionProduct,
                    NameCategorie = p.TagCategorieNavigation.NameCategorie,
                    TagCategorie = p.TagCategorie,
                    StockQuantity = p.Inventory != null ? p.Inventory.StockQuantity : 0,
                    StockBajo = p.Inventory != null && p.Inventory.StockQuantity <= p.Inventory.MinQuantity
                })
                .ToListAsync();

        public async Task<ProductoDetalleDTO?> GetByIdAsync(int id) =>
            await _context.Products
                .Where(p => p.TagProduct == id && p.IsActive)
                .Include(p => p.TagCategorieNavigation)
                .Include(p => p.Inventory)
                .Select(p => new ProductoDetalleDTO
                {
                    TagProduct = p.TagProduct,
                    NameProduct = p.NameProduct,
                    UnitPrice = p.UnitPrice,
                    DescriptionProduct = p.DescriptionProduct,
                    NameCategorie = p.TagCategorieNavigation.NameCategorie,
                    TagCategorie = p.TagCategorie,
                    StockQuantity = p.Inventory != null ? p.Inventory.StockQuantity : 0,
                    StockBajo = p.Inventory != null && p.Inventory.StockQuantity <= p.Inventory.MinQuantity
                })
                .FirstOrDefaultAsync();
    }
}
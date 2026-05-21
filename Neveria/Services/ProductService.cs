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
                    TagProduct         = p.TagProduct,
                    NameProduct        = p.NameProduct,
                    UnitPrice          = p.UnitPrice,
                    DescriptionProduct = p.DescriptionProduct,
                    NameCategorie      = p.TagCategorieNavigation.NameCategorie,
                    TagCategorie       = p.TagCategorie,
                    StockQuantity      = p.Inventory != null ? p.Inventory.StockQuantity : 0,
                    StockBajo          = p.Inventory != null && p.Inventory.StockQuantity <= p.Inventory.MinQuantity
                })
                .ToListAsync();

        public async Task<ProductoDetalleDTO?> GetByIdAsync(int id) =>
            await _context.Products
                .Where(p => p.TagProduct == id && p.IsActive)
                .Include(p => p.TagCategorieNavigation)
                .Include(p => p.Inventory)
                .Select(p => new ProductoDetalleDTO
                {
                    TagProduct         = p.TagProduct,
                    NameProduct        = p.NameProduct,
                    UnitPrice          = p.UnitPrice,
                    DescriptionProduct = p.DescriptionProduct,
                    NameCategorie      = p.TagCategorieNavigation.NameCategorie,
                    TagCategorie       = p.TagCategorie,
                    StockQuantity      = p.Inventory != null ? p.Inventory.StockQuantity : 0,
                    StockBajo          = p.Inventory != null && p.Inventory.StockQuantity <= p.Inventory.MinQuantity
                })
                .FirstOrDefaultAsync();

        // Devuelve la entidad cruda para las vistas CRUD (necesitan el objeto completo)
        public async Task<List<Product>> GetAllRawAsync() =>
            await _context.Products
                .Include(p => p.TagCategorieNavigation)
                .ToListAsync();

        public async Task CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(int id, Product product)
        {
            if (id != product.TagProduct) return false;
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            product.IsActive = false;          // ← solo desactiva
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Products.AnyAsync(p => p.TagProduct == id);
    }
}

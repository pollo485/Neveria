using Microsoft.EntityFrameworkCore;
using Neveria.Models.dbFreezeDream;
using Neveria.Models.DTOs;

namespace Neveria.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DbFreezeDreamContext _context;
        public CategoryService(DbFreezeDreamContext context) => _context = context;

        public async Task<List<CategoriesDTO>> GetAllAsync() =>
            await _context.Categories
                .Select(c => new CategoriesDTO
                {
                    CategoryId          = c.TagCategorie,
                    CategoryName        = c.NameCategorie,
                    CategoryDescription = c.DescriptionCategorie
                })
                .ToListAsync();

        public async Task<CategoriesDTO?> GetByIdAsync(int id) =>
            await _context.Categories
                .Where(c => c.TagCategorie == id)
                .Select(c => new CategoriesDTO
                {
                    CategoryId          = c.TagCategorie,
                    CategoryName        = c.NameCategorie,
                    CategoryDescription = c.DescriptionCategorie
                })
                .FirstOrDefaultAsync();

        public async Task CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(int id, Category category)
        {
            if (id != category.TagCategorie) return false;
            try
            {
                _context.Update(category);
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
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Categories.AnyAsync(c => c.TagCategorie == id);
    }
}

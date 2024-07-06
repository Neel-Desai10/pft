using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _categoryContext;
        public CategoryRepository(ApplicationDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public async Task<List<CategoryDto>> GetCategory(int categoryTypeId)
        {
            return await _categoryContext.Categories
            .Where(c => c.CategoryTypeId == categoryTypeId)
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Category = c.Category,
            })
            .ToListAsync();
        }
    }
}
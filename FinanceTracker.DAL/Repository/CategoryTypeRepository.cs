using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository;

public class CategoryTypeRepository : ICategoryTypeRepository
{
    private readonly ApplicationDbContext _categoryTypeContext;
    public CategoryTypeRepository(ApplicationDbContext categoryTypeContext)
    {
        _categoryTypeContext = categoryTypeContext;
    }
    public async Task<List<CategoryTypeDto>> GetCategoryTypeModel()
    {
        var data = await _categoryTypeContext.CategoryTypes
        .Select(x => new CategoryTypeDto
        {
            CategoryTypeId = x.CategoryTypeId,
            CategoryType = x.CategoryType
        })
        .OrderBy(x => x.CategoryTypeId).ToListAsync();
        return data;
    }


}

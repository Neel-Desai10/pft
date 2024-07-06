using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto> GetCategories(int categoryTypeId)
        {
            var data = new CategoryResponseDto();
            data.CategoriesList = await _categoryRepository.GetCategory(categoryTypeId);
            return data;
            
        }

    }

}


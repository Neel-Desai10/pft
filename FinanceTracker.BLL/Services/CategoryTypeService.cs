using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services;

public class CategoryTypeService : ICategoryTypeService
{
    private readonly ICategoryTypeRepository _categoryTypeRepository;
    public CategoryTypeService(ICategoryTypeRepository categoryTypeRepository)
    {
        _categoryTypeRepository = categoryTypeRepository;
    }

    public async Task<CategoryTypeResponse> GetCategoryType()

    {
        var response = new CategoryTypeResponse();
        response.CategoryTypesList  = await _categoryTypeRepository.GetCategoryTypeModel();
        return response;
    }

}

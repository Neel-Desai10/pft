using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface;

public interface ICategoryTypeService
{
     Task<CategoryTypeResponse> GetCategoryType();
}

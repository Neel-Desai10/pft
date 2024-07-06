using FinanceTracker.DAL.Dtos;


namespace FinanceTracker.DAL.Interface;

public interface ICategoryTypeRepository
{
     Task<List<CategoryTypeDto>> GetCategoryTypeModel();
}

using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> GetCategories(int categoryTypeId);
    }
}
using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.DAL.Interface
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetCategory(int categoryTypeId);
    }
}
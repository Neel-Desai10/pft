using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface;

public interface IStatusServices
{
    Task<StatusModelDto> GetStatuses();
}

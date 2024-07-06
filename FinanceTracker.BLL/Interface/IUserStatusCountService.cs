using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface IUserStatusCountService
    {
        Task<UserStatusCountResponseDto> UserStatusCount();
    }
}
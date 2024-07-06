using FinanceTracker.DAL.DTO;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.Utility.Enum;

namespace FinanceTracker.BLL.Interface
{
    public interface IAdminService
    {
        Task<UserListDto> GetUserDetailByStatus(UserStatusListEnum userStatusId, PaginationParameters paginationParams,string searchParam);
        Task<string> UpdateUserStatusId(List<ApprovalRequestDto> userStatusUpdates,int loginUserId);
        Task<string> UpdateUserAccountStatus(int userId, bool isActive, int loggedInUserId);
        Task<ActivityChartsResponseDto> GetActivityCharts(int year, int month);
    }
}
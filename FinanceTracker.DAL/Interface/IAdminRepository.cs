using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.Utility.Enum;

namespace FinanceTracker.DAL.Interface
{
    public interface IAdminRepository
    {
        Task<UserListDto> GetUserDetail(UserStatusListEnum userStatusId, PaginationParameters paginationParams,string searchParam);
        Task<int> GetTotalUsersCount(byte userStatusId);
        Task<UserModel> GetUserById(int userId);
        Task<int> GetRoleByUserId(int userId);
        Task<bool> UpdateUserStatus(UserModel userModel);
        Task<int?> FindUserIdByRolId(int rolId);
        Task<List<UserModel>> GetActivityCharts(int year, int month);
    }
}
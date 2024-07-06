using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility.Enum;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{ 
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _UserContext;
        public AdminRepository(ApplicationDbContext userContext)
        {
            _UserContext = userContext;
        }
        public async Task<UserListDto> GetUserDetail(UserStatusListEnum userStatusId, PaginationParameters paginationParams, string searchParam)
        {
            var data = new List<object>();
            var response = new UserListDto();
            List<UserDto> userDetails = await UserDetails();
            var filteredUsers = userDetails
            .Where(x => x.UserStatusId == (int)userStatusId);
            if (!string.IsNullOrEmpty(searchParam))
            {
                filteredUsers = filteredUsers
                                .Where(x => x.FirstName.Contains(searchParam, StringComparison.OrdinalIgnoreCase) ||
                                            x.EmailId.Contains(searchParam, StringComparison.OrdinalIgnoreCase));
            }
            GetUsersList(paginationParams, response, filteredUsers);
            response.TotalRecordCount = filteredUsers.Count();
            return response;
        }
        private static void GetUsersList(PaginationParameters paginationParams, UserListDto response, IEnumerable<UserDto> filteredUsers)
        {
            response.UsersList = filteredUsers
                                            .OrderByDescending(x => x.StatusTimestamp)
                                            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                            .Take(paginationParams.PageSize)
                                            .Select(v => new UserDto()
                                            {
                                                UserId = v.UserId,
                                                ModifiedDate = v.ModifiedDate,
                                                FirstName = v.FirstName,
                                                LastName = v.LastName,
                                                isActive = v.isActive,
                                                PhoneNumber = v.PhoneNumber,
                                                EmailId = v.EmailId,
                                                RejectionReason = v.RejectionReason,
                                                UserStatusId = v.UserStatusId,
                                                StatusTimestamp = v.StatusTimestamp
                                            })
                                            .ToList();
        }
        private async Task<List<UserDto>> UserDetails()
        {
            return await _UserContext.UserData.Where(x => x.RoleId == (int)Role.Employee)
                            .OrderByDescending(x => x.UserId)
                            .Select(x => new UserDto()
                            {
                                UserId = x.UserId,
                                ModifiedDate = x.ModifiedDate,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                EmailId = x.EmailId,
                                PhoneNumber = x.PhoneNumber,
                                isActive = x.IsActive,
                                RejectionReason = x.RejectionReason,
                                UserStatusId = x.UserStatusId,
                                StatusTimestamp = x.StatusTimestamp
                            })
                            .ToListAsync();
        }
        public async Task<int> GetTotalUsersCount(byte userStatusId)
        {
            return await _UserContext.UserData
                .Where(x => x.UserStatusId == userStatusId)
                .CountAsync();
        }
        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await _UserContext.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
            return user;
        }

        public async Task<int> GetRoleByUserId(int userId)
        {
            var user = await _UserContext.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
            return user.RoleId;
        }

        public async Task<bool> UpdateUserStatus(UserModel userModel)
        {
            try
            {
                _UserContext.Update(userModel);
                await _UserContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int?> FindUserIdByRolId(int rolId)
        {
            var user = await _UserContext.UserData.FirstOrDefaultAsync(x => x.RoleId == rolId);
            return user?.UserId;
        }
        public async Task<List<UserModel>> GetActivityCharts(int year, int month)
        {
            var users = await _UserContext.UserData
                .Where(x => x.CreatedDate.Year == year && (month == ResponseResources.Zero || x.CreatedDate.Month == month))
                .ToListAsync();
            return users;
        }
    }
}
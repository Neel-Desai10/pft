using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class UserStatusCountService : IUserStatusCountService
    {
        private IUserStatusCountRepository _userStatusCountRepository;

        public UserStatusCountService(IUserStatusCountRepository userStatusCountRepository)
        {
            _userStatusCountRepository = userStatusCountRepository;
        }

        public async Task<UserStatusCountResponseDto> UserStatusCount()
        {
            var users = await _userStatusCountRepository.GetAllUsers();
            return new UserStatusCountResponseDto
            {
                PendingUsers = users.Count(x => x.UserStatusId == (int)UserStatusEnum.Pending),
                ApprovedUsers = users.Count(x => x.UserStatusId == (int)UserStatusEnum.Approved),
                RejectedUsers = users.Count(x => x.UserStatusId == (int)UserStatusEnum.Rejected),
                ActiveUsers = users.Count(x => x.UserStatusId == (int)UserStatusEnum.Approved && x.IsActive == true),
                InActiveUsers = users.Count(x => x.UserStatusId == (int)UserStatusEnum.Approved && x.IsActive == false)
            };
        }
    }
}
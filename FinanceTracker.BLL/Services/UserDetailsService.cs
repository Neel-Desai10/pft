using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailsRepository _userDetailsRepository;
        public UserDetailsService(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }
        public async Task<UserDetailsResponseDto> GetUserDetails(int userId)
        {
            var userDetails = await _userDetailsRepository.GetUserDetails(userId);
            return userDetails;
        }
        public UserResponseDto GetUserIdBySubjectId(string subjectId)
        {
            return _userDetailsRepository.GetUserIdBySubjectId(subjectId);
        }
    }
}
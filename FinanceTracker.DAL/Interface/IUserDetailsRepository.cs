using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.DAL.Interface
{
    public interface IUserDetailsRepository
    {
        public Task<UserDetailsResponseDto> GetUserDetails(int userId);
        UserResponseDto GetUserIdBySubjectId(string subjectId);
    }
}
using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface IUserDetailsService
    {
        Task<UserDetailsResponseDto> GetUserDetails(int userId); 
        UserResponseDto GetUserIdBySubjectId(string subjectId);
    }
}
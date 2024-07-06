using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;

namespace FinanceTracker.DAL;

public interface IUserServices
{
    Task<bool> IsEmailExist(string email);
    Task RegisterUserWithAuthority(UserRequestDto userRequest);
    Task<UserDetailDto> GetUserBySubjectId(string subjectId);
    Task<bool> IsSubjectIdExistService(string subjectId);
    Task<TransactionResponseDto> GetTotalTransactionsByYear(int userId,int year);
}

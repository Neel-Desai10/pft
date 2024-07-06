using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL;

public interface IUserRepository
{
    Task<UserRequestDto> AddUser(UserRequestDto users);
    Task<bool> IsEmailAlreadyExist(string email);
    Task<UserDetailDto> GetUser(string subjectId);
    Task<bool> IsSubjectIdExist(string subjectId);
   Task<List<TransactionModel>> GetTransactionsByYear(int userId, int year);
}

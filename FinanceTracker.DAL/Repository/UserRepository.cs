using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;
namespace FinanceTracker.DAL;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _UserContext;
    public UserRepository(ApplicationDbContext userContext)
    {
        _UserContext = userContext;
    }
    public async Task<UserRequestDto> AddUser(UserRequestDto users)
    {
        var data = users.signupDTOMapper();
        await _UserContext.AddAsync(data);
        await _UserContext.SaveChangesAsync();
        var user = await _UserContext.UserData.FirstOrDefaultAsync(u => u.SubjectId == users.SubjectId);
        users.UserId = user.UserId;
        return users;
    }
    public async Task<bool> IsEmailAlreadyExist(string email)
    {
        return await _UserContext.UserData.AnyAsync(u => u.EmailId == email);
    }
    public async Task<UserDetailDto> GetUser(string subjectId)
    {
        var data = await _UserContext.UserData
            .Where(x => x.SubjectId == subjectId)
            .Select(x => x.UserDetailDTOMapper())
            .FirstOrDefaultAsync();
        return data;
    }
    public async Task<bool> IsSubjectIdExist(string subjectId)
    {
        return await _UserContext.UserData.AnyAsync(u => u.SubjectId == subjectId);
    }

    public async Task<List<TransactionModel>> GetTransactionsByYear(int userId, int year)
    {
        var transactions = await _UserContext.Transactions
            .Where(t => t.CreatedBy == userId && t.DeletedDate == null && t.TransactionDate.Year == year)
            .Include(t => t.Category).ThenInclude(c => c.CategoryType).ToListAsync();

        return transactions;
    }
}

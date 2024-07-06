using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class AdminCreateUserRepository : IAdminCreateUserRepository
    {
        private readonly ApplicationDbContext _userContext;

        public AdminCreateUserRepository(ApplicationDbContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<UserModel> CreateUserFromAdmin(CreateUserDto createUser)
        {
            var data = createUser.CreateUsersDtoMapper();
            await _userContext.AddAsync(data);
            await _userContext.SaveChangesAsync();
            return data;
        }
        public async Task<bool> IsEmailAlreadyExist(string email)
        {
            return await _userContext.UserData.AnyAsync(u => u.EmailId == email);
        }
        public async Task<int?> FindUserIdByRoleId(int roleId)
        {
            var user = await _userContext.UserData.FirstOrDefaultAsync(u => u.RoleId == roleId);
            return user?.UserId;
        }
    }
}
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class UserStatusCountRepository : IUserStatusCountRepository
    {
        private ApplicationDbContext _statusCountDbContext;

        public UserStatusCountRepository(ApplicationDbContext statusCountDbContext)
        {
            _statusCountDbContext = statusCountDbContext;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _statusCountDbContext.UserData.ToListAsync();
        }
    }
}
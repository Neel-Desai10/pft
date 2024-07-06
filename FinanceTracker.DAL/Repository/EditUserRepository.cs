using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class EditUserRepository : IEditUserRepository
    {
        private readonly ApplicationDbContext _editUserDbContext;

        public EditUserRepository(ApplicationDbContext editUserDbContext)
        {
            _editUserDbContext = editUserDbContext;
        }

        public async Task<UserModel> GetUserById(int userId)
        {

            return await _editUserDbContext.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task EditUser(UserModel user)
        {
            _editUserDbContext.UserData.Update(user);
            await _editUserDbContext.SaveChangesAsync();
        }
    }
}
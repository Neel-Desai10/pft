using FinanceTracker.DAL.Dtos.AdminDto;

namespace FinanceTracker.DAL.Interface
{
    public interface IAdminCreateUserRepository
    {
        Task<UserModel> CreateUserFromAdmin(CreateUserDto createUser);
        Task<bool> IsEmailAlreadyExist(string email);
        Task<int?> FindUserIdByRoleId(int roleId);
    }
}
using FinanceTracker.DAL.Dtos.AdminDto;

namespace FinanceTracker.BLL.Interface
{
    public interface IAdminCreateUserServices
    {
        Task AdminCreateUser(CreateUserDto createUser,int loginUserId);
        Task<bool> IsEmailExist(string email);
    }
}
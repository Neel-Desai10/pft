using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface IEditUserService
    {
        Task EditUsers(int userId , EditUserDto editUserDto,int loggedInUserId); 
    }
}
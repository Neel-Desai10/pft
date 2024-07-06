namespace FinanceTracker.DAL.Interface
{
    public interface IEditUserRepository
    {
        Task<UserModel> GetUserById(int userId);
        Task EditUser(UserModel user);
    }
}
namespace FinanceTracker.DAL.Interface
{
    public interface IUserStatusCountRepository
    {
        Task<List<UserModel>> GetAllUsers();
    }
}
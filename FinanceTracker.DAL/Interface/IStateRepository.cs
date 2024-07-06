using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface IStateRepository
    {
        public Task<List<StateModel>> GetAllStates(int countryId);
    }
}
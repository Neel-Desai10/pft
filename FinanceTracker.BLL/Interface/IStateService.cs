using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface IStateService
    {
        Task<StateDto> GetAllStates(int countryId); 
        
    }   
}
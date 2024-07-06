using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
         public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<StateDto> GetAllStates(int countryId)
        {
            var data = new StateDto();
            data.StatesList = await _stateRepository.GetAllStates(countryId);
            return data;
        }
    }
}
using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface;

public interface ICityService
{
    Task<CityDto> GetCities(int stateId); 
}

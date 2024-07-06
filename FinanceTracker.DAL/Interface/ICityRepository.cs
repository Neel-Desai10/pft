using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface;

public interface ICityRepository
{
    Task<List<CityModel>> GetCityModel(int stateId);
}

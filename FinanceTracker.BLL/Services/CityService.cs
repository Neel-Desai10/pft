using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<CityDto> GetCities(int stateId)
    {
        var data = new CityDto();
        data.CitiesList = await _cityRepository.GetCityModel(stateId);
        return data;
     
    }

}

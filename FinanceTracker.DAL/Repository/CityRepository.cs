using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _cityContext;
    public CityRepository(ApplicationDbContext cityContext)
    {
        _cityContext = cityContext;
    }

    public async Task<List<CityModel>> GetCityModel(int stateId)
    {
       return await _cityContext.Cities.Where(x=>x.StateId == stateId).ToListAsync();
    }
}

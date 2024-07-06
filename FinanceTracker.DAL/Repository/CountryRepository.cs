using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _countryContext;
    public CountryRepository(ApplicationDbContext countryContext)
    {
        _countryContext = countryContext;
    }
    public async Task<List<CountryModel>> GetCountry()
    {
        var data = await _countryContext.Countries.ToListAsync();
        return data;
    }
    }
}
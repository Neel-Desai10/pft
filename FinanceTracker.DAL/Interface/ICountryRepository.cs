using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface ICountryRepository
    {
        Task<List<CountryModel>> GetCountry();
    }
}
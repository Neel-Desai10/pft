using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface ICountryServices
    {
        Task<CountryDto> GetCountries();
    }
}
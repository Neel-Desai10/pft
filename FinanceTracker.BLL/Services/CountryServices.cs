using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class CountryServices : ICountryServices
    {
        private readonly ICountryRepository _countryRepository;
        public CountryServices(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<CountryDto> GetCountries()
        {
            var data = new CountryDto();
            data.CountriesList = await _countryRepository.GetCountry();
            return data;
        }
    }
}
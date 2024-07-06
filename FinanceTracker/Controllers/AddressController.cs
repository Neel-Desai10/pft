using FinanceTracker.BLL.Interface;
using FinanceTracker.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers
{
    [Authorize(Roles = "Employee")]
    [ApiController]
    public class AddressController : BaseResponse
    {
        private readonly ICountryServices _countryService;
        private readonly ICityService _cityServices;
        private readonly IStateService _stateService;
        private readonly ILogger<RolesController> _logger;
        public AddressController(IStateService stateService, ICountryServices countryService, ICityService cityServices, ILogger<RolesController> logger)
        {
            _countryService = countryService;
            _cityServices = cityServices;
            _stateService = stateService;
            _logger = logger;
        }
        [Route("countries")]
        [HttpGet]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var countries = await _countryService.GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }
        [Route("cities/{stateId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllCities(int stateId)
        {
            try
            {
                var cities = await _cityServices.GetCities(stateId);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }
        [Route("states/{countryId}")]
        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId)
        {
            try
            {
                var states = await _stateService.GetAllStates(countryId);
                return Ok(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }
    }
}
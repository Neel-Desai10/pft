using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IEditUserService _editUserService;
        private readonly IUserServices _userServices;
        private readonly ICategoryTypeService _categoryTypeServices;
        private readonly ILogger<UserController> _logger;
        private readonly ICategoryService _categoryServices;
        private readonly IChangePasswordService _changePasswordService;
        private readonly ITransactionSummaryService _transactionSummaryService;

       public UserController(IEditUserService editUserService, IUserServices userServices, ICategoryService categoryServices, IChangePasswordService changePasswordService, ICategoryTypeService categoryTypeServices, ILogger<UserController> logger, ITransactionSummaryService transactionSummaryService,IUserDetailsService userDetailsService) : base (userDetailsService)
        {
            _editUserService = editUserService;
            _userServices = userServices;
            _categoryServices = categoryServices;
            _changePasswordService = changePasswordService;
            _logger = logger;
            _categoryTypeServices = categoryTypeServices;
            _transactionSummaryService = transactionSummaryService;
        }

        [Route("users/register")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto userRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errorMessage.Append($"{error.ErrorMessage}; ");
                    }
                    _logger.LogWarning($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                await _userServices.RegisterUserWithAuthority(userRequestDto);
                _logger.LogInformation(ValidationResources.RegistrationSuccess);
                return Ok(ValidationResources.RegistrationSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("categories/{categoryTypeId}")]
        [HttpGet]
        public async Task<IActionResult> Get(int categoryTypeId)
        {
            try
            {
                GetUserId();
                var response = await _categoryServices.GetCategories(categoryTypeId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("users/{subjectId}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string subjectId)
        {
            try
            {
                GetUserId();
                if (await _userServices.IsSubjectIdExistService(subjectId))
                {
                    var userDetail = await _userServices.GetUserBySubjectId(subjectId);
                    return Ok(userDetail);
                }
                else
                {
                    return Error(ResponseResources.NoUserFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("users/{userId}/edit-profile")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] EditUserDto editUserDto)
        {
            try
            {
                var loggedInUserId = GetUserId();
                if (!ModelState.IsValid)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errorMessage.Append($"{error.ErrorMessage}; ");
                    }
                    _logger.LogWarning($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                await _editUserService.EditUsers(userId, editUserDto,loggedInUserId);
                _logger.LogInformation(ValidationResources.UpdateUserSuccessful);
                return Ok(ValidationResources.UpdateUserSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("categoryTypes")]
        [HttpGet]
        public async Task<IActionResult> GetCategoryByType()
        {
            try
            {
                GetUserId();
                var response = await _categoryTypeServices.GetCategoryType();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("users/{subjectId}/change-password")]
        [HttpPut]
        public async Task<IActionResult> UserChangePassword(string subjectId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                GetUserId();
                if (!ModelState.IsValid)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errorMessage.Append($"{error.ErrorMessage}; ");
                    }
                    _logger.LogWarning($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                var response = await _changePasswordService.ChangePassword(subjectId, changePasswordDto);
                if (response.IsSuccessful)
                {
                    return Ok(ValidationResources.PasswordChangedSuccessfully);
                }
                else
                {
                    return Error(ValidationResources.PasswordMismatch);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("users/{userId}/dashboard/total-transaction-details-by-year")]
        [HttpGet]
        public async Task<IActionResult> GetTotalTransactionsByYear(int userId, [FromQuery][Required] int year)
        {
            try
            {
                GetUserId();
                var response = await _userServices.GetTotalTransactionsByYear(userId, year);
                _logger.LogInformation(ResponseResources.LogSuccessRetrievingTransactions);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Employee")]
        [Route("users/{userId}/dashboard/total-transaction-details-by-month")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionsSummary(int userId, [FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                GetUserId();
                var transactions = await _transactionSummaryService.GetTransactionSummary(userId, year, month);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }
    }
}

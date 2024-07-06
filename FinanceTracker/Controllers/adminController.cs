using FinanceTracker.BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.DTO;
using FinanceTracker.DAL.Resources;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.Utility;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using FinanceTracker.Utility.Enum;

namespace FinanceTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminServices;
        private readonly IUserDetailsService _userDetailsServices;
        private readonly IUserStatusCountService _userStatusCountService;
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminCreateUserServices _adminCreateUserServices;
        public AdminController(IAdminCreateUserServices adminCreateUserServices, IAdminService adminServices, ILogger<AdminController> logger, IUserStatusCountService userStatusCountService, IUserDetailsService userDetailsService) : base(userDetailsService)
        {

            _logger = logger;
            _adminServices = adminServices;
            _adminCreateUserServices = adminCreateUserServices;
            _userStatusCountService = userStatusCountService;
            _userDetailsServices = userDetailsService;
        }

        [Route("admin/users/{statusId}")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetUsersList(UserStatusListEnum statusId, [FromQuery] PaginationParameters paginationParams, string searchParam)
        {
            try
            {
                GetUserId();
                var response = await _adminServices.GetUserDetailByStatus(statusId, paginationParams, searchParam);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return Error(ValidationResources.NoContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.NoData);
                return Error(ex.Message.ToString());
            }
        }

        [HttpPost("admin/registration-requests")]
        public async Task<IActionResult> UpdateUsersStatus(List<ApprovalRequestDto> userStatus)
        {
            try
            {
                var loggedInUserId = GetUserId();
                var result = await _adminServices.UpdateUserStatusId(userStatus, loggedInUserId);
                _logger.LogInformation(ResponseResources.UserUpdateSuccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.UserUpdateError);
                return Error(ex.Message.ToString());
            }
        }

        [HttpPost("admin/create-user")]
        public async Task<IActionResult> AdminCreateUser([FromBody] CreateUserDto createUser)
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
                    _logger.LogError($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                else
                {
                    await _adminCreateUserServices.AdminCreateUser(createUser, loggedInUserId);
                    _logger.LogInformation(ValidationResources.AdminCreateUserRegistrationSuccess);
                    return Ok(ValidationResources.AdminCreateUserRegistrationSuccess);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }
        [Route("admin/{userId}/details")]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            try
            {
                GetUserId();
                var userDetails = await _userDetailsServices.GetUserDetails(userId);
                if (userDetails.UserId != null)
                {
                    return Ok(userDetails);
                }
                else
                {
                    return Error(ResponseResources.NoUserFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.UserDetailsError);
                return Error(ex.Message.ToString());
            }
        }

        [HttpPut("admin/{userID}/account-status")]
        public async Task<IActionResult> UpdateUserAccountStatus(int userID, [FromQuery][Required] bool isActive)
        {
            try
            {
                var loggedInUserId = GetUserId();
                var response = await _adminServices.UpdateUserAccountStatus(userID, isActive, loggedInUserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [HttpGet("admin/user-reports/user-status-count")]
        public async Task<IActionResult> GetUserStatusCount()
        {
            try
            {
                GetUserId();
                var userStatusCount = await _userStatusCountService.UserStatusCount();
                return Ok(userStatusCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Route("admin/user-reports/activity-charts")]
        [HttpGet]
        public async Task<IActionResult> ActivityCharts([FromQuery][Required] int year, [FromQuery][Required] int month)
        {
            try
            {
                GetUserId();
                var response = await _adminServices.GetActivityCharts(year, month);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text;
using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[Authorize(Roles = "Employee")]
[ApiController]
public class CalendarController : BaseController
{
    private readonly ILogger<CalendarController> _logger;
    private readonly IReminderAlertService _reminderAlertService;
    private readonly IReminderService _reminderService;

    public CalendarController(ILogger<CalendarController> logger, IReminderAlertService reminderAlertService, IReminderService reminderService, IUserDetailsService userDetailsService) : base(userDetailsService)
    {
        _logger = logger;
        _reminderAlertService = reminderAlertService;
        _reminderService = reminderService;
    }
    [Route("users/{userId}/reminder/{reminderId}")]
    [HttpPut]
    public async Task<IActionResult> UpdateReminder(int userId, int reminderId, ReminderDto reminderResponse)
    {
        try
        {
            var loggedInUserId = GetUserId();
            var result = await _reminderService.UpdateReminder(loggedInUserId, reminderId, reminderResponse);
            _logger.LogInformation(ValidationResources.ReminderUpdatedSuccessfully);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message.ToString());
            return Error(ex.Message.ToString());
        }
    }
    [HttpGet("reminder-alert")]
    public async Task<IActionResult> GetAllReminderAlert()
    {
        try
        {
            GetUserId();
            var reminderAlerts = await _reminderAlertService.GetAllReminderAlert();
            return Ok(reminderAlerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return Error(ex.Message.ToString());
        }
    }

    [HttpPost("users/{userId}/reminder")]
    public async Task<IActionResult> AddReminder(int userId, ReminderDto reminderRequest)
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
            var result = await _reminderService.CreateReminder(loggedInUserId, reminderRequest);
            _logger.LogInformation(ResponseResources.SetReminderSuccessfully);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message.ToString());
            return Error(ex.Message.ToString());
        }
    }

    [HttpGet("/users/{userId}/reminder-count-by-month")]
    public async Task<IActionResult> GetMonthWiseReminder(int userId, [Required][FromQuery] int month, [Required][FromQuery] int year)
    {
        try
        {
            GetUserId();
            var reminderList = await _reminderService.MonthWiseReminder(userId, month, year);

            _logger.LogInformation(ValidationResources.ReminderDataSuccess);
            return Ok(reminderList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message.ToString());
            return Error(ex.Message.ToString());
        }
    }


    [HttpGet("/users/{userId}/reminder-details/{reminderId}")]
    public async Task<IActionResult> GetReminderDetailsById(int userId, int reminderId)
    {
        try
        {
            GetUserId();
            var reminderDetails = await _reminderService.GetReminderDetailsById(userId, reminderId);
            if (reminderDetails.ReminderId != 0)
            {
                _logger.LogInformation(ResponseResources.ReminderDetailFetchSuccess);
                return Ok(reminderDetails);
            }
            else
            {
                _logger.LogError(ResponseResources.ReminderError);
                return Error(ResponseResources.NoData);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ResponseResources.ReminderError);
            return Error(ex.Message.ToString());
        }
    }

    [HttpGet("users/{userId}/reminder-list")]
    public async Task<IActionResult> GetDateWiseReminder(int userId, [Required][FromQuery] int month, [Required][FromQuery] int year, [Required][FromQuery] int date)
    {
        try
        {
            GetUserId();
            var result = await _reminderService.DateWiseReminder(userId, month, year, date);
            _logger.LogInformation(ResponseResources.ReminderFetchSuccess);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ResponseResources.ReminderError);
            return Error(ex.Message.ToString());
        }
    }
    [HttpDelete("users/{userId}/reminder/{reminderId}")]
    public async Task<IActionResult> DeleteReminder(int userId, int reminderId)
    {
        try
        {
            var loggedInUserId = GetUserId();
            var result = await _reminderService.DeleteReminder(loggedInUserId, reminderId);
            _logger.LogInformation(ValidationResources.ReminderDeletedSuccessfully);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message.ToString());
            return Error(ex.Message.ToString());
        }
    }
}
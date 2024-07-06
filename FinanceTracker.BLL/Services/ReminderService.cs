using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos.ReminderDtos;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Hangfire.Interface;
using FinanceTracker.Utility.Interface;
using FinanceTracker.Utility.Model;
using FinanceTracker.BLL.Utility;
using Microsoft.Extensions.Logging;
using FinanceTracker.DAL.Model;
using FinanceTracker.Hangfire.Model;
using Microsoft.Extensions.Options;

namespace FinanceTracker.BLL.Services
{
   public class ReminderService : IReminderService
   {
      private readonly IReminderRepository _reminderRepository;
      private readonly IBackgroundJobs _backgroundJobs;
      private readonly IMailService _mailService;
      private readonly ILogger<ReminderService> _logger;
      private readonly HangfireSettings _hangfireSettings;
      public ReminderService(IReminderRepository reminderRepository, IBackgroundJobs backgroundJobs, IMailService mailService, ILogger<ReminderService> logger, IOptions<HangfireSettings> hangfireSettings)
      {
         _reminderRepository = reminderRepository;
         _backgroundJobs = backgroundJobs;
         _mailService = mailService;
         _logger = logger;
         _hangfireSettings = hangfireSettings.Value;
      }

      public async Task<string> CreateReminder(int userId, ReminderDto reminderResponse)
      {
         DateTime currentTime;
         Dictionary<int, TimeSpan> alertOffsets;
         ReminderAlertCheck(out currentTime, out alertOffsets);
         if (!alertOffsets.TryGetValue(reminderResponse.ReminderAlertId, out var offset))
         {
            throw new Exception(ValidationResources.ReminderCannotSet);
         }
         var reminderTime = reminderResponse.ReminderDate - offset - currentTime;
         if (reminderTime >= TimeSpan.Zero)
         {
            await _reminderRepository.CreateReminder(userId, reminderResponse);
            return ResponseResources.ReminderSetSuccessfully;
         }
         else
         {
            throw new Exception(ValidationResources.ReminderCannotSet);
         }
      }

      private static void ReminderAlertCheck(out DateTime currentTime, out Dictionary<int, TimeSpan> alertOffsets)
      {
         currentTime = DateTime.Now;
         alertOffsets = new Dictionary<int, TimeSpan>
         {
            { 1, TimeSpan.Zero },
            { 2, TimeSpan.FromMinutes(Resources.AddMinutes) },
            { 3, TimeSpan.FromHours(Resources.AddHours) },
            { 4, TimeSpan.FromDays(Resources.AddDays) }
         };
      }
      public async Task<ReminderDetailDto> GetReminderDetailsById(int userId, int reminderId)
      {
         return await _reminderRepository.GetReminderDetailsById(userId, reminderId);
      }

      public async Task<ReminderResponseDto> DateWiseReminder(int userId, int month, int year, int date)
      {
         var reminders = new ReminderResponseDto();
         reminders.ReminderList = await _reminderRepository.DateWiseReminderList(userId, month, year, date);
         return reminders;
      }


      public async Task<ReminderDtoList> MonthWiseReminder(int userId, int month, int year)
      {
         List<ReminderDateTimeDto> reminderDetails = await _reminderRepository.GetReminderDetails(userId, month, year);

         var response = new ReminderDtoList();

         response.ReminderCountData = reminderDetails
        .GroupBy(r => DateOnly.FromDateTime(r.ReminderDateTime))
        .Select(g => new ReminderDateCountDto
        {
           ReminderDate = g.Key,
           ReminderCount = g.Count()
        })
        .ToList();

         return response;
      }

      public async Task<string> UpdateReminder(int userId, int reminderId, ReminderDto reminderResponse)
      {
         var existingReminder = await _reminderRepository.GetReminderByReminderId(userId, reminderId);
         if (existingReminder is null)
         {
            throw new Exception(ValidationResources.ReminderIdNotFound);
         }
         DateTime currentTime;
         Dictionary<int, TimeSpan> alertOffsets;
         ReminderAlertCheck(out currentTime, out alertOffsets);
         if (!alertOffsets.TryGetValue(reminderResponse.ReminderAlertId, out var offset))
         {
            throw new Exception(ValidationResources.ReminderCannotSet);
         }
         var reminderTime = reminderResponse.ReminderDate - offset - currentTime;
         if (reminderTime >= TimeSpan.Zero && existingReminder.ReminderDate >= DateTime.Now && existingReminder.EmailSentTime is null)
         {
            existingReminder.Value = reminderResponse.Value;
            existingReminder.Title = reminderResponse.Title;
            existingReminder.ReminderDate = reminderResponse.ReminderDate;
            existingReminder.ReminderAlertId = reminderResponse.ReminderAlertId;
            existingReminder.Notes = reminderResponse.Notes;
            existingReminder.ModifiedDate = DateTime.Now;
            existingReminder.ModifiedBy = userId;
            await _reminderRepository.UpdateReminderByUserId(existingReminder);
            return ValidationResources.ReminderUpdatedSuccessfully;
         }
         else
         {
            throw new Exception(ValidationResources.ReminderCannotSet);
         }
      }
      public async Task<string> DeleteReminder(int userId, int reminderId)
      {
         var reminderData = await _reminderRepository.GetReminderByReminderId(userId, reminderId);
         if (reminderData is null)
         {
            throw new Exception(ValidationResources.ReminderIdNotFound);
         }
         reminderData.DeletedBy = userId;
         reminderData.DeletedDate = DateTime.Now;
         await _reminderRepository.DeleteReminderByReminderId(reminderData);
         return ValidationResources.ReminderDeletedSuccessfully;
      }
      public async Task CheckReminderEmailToSend()
      {
         var reminderList = await _reminderRepository.CheckMailSend();
         if (reminderList is null)
         {
            _logger.LogInformation(Resources.NoMailToSend);
            return;
         }
         foreach (var reminder in reminderList)
         {
            DateTime mailSendTime = CalculateMailSendTime(reminder.ReminderDate, reminder.ReminderAlertId);
            if (mailSendTime <= DateTime.Now && mailSendTime > DateTime.Now.AddMinutes(_hangfireSettings.LastRecur))
            {
               await ProcessReminder(reminder);
            }
         }
      }
      private async Task ProcessReminder(ReminderModel reminder)
      {
         if (await MailSendProcess(reminder))
         {
            await _reminderRepository.MailSent(reminder.ReminderId);
         }
         await Task.Delay(100);
      }
      private DateTime CalculateMailSendTime(DateTime reminderDate, int reminderAlertId)
      {
         var reminderActions = new Dictionary<int, Func<DateTime, DateTime>>
         {
            { 1, reminderDate => reminderDate },
            { 2, reminderDate => reminderDate.AddMinutes(-Resources.AddMinutes) },
            { 3, reminderDate => reminderDate.AddHours(-Resources.AddHours) },
            { 4, reminderDate => reminderDate.AddDays(-Resources.AddDays) }
         };
         return reminderActions.TryGetValue(reminderAlertId, out var mailSendTime) ? mailSendTime(reminderDate) : reminderDate;
      }
      private async Task<bool> MailSendProcess(ReminderModel reminder)
      {
         string formattedDate = reminder.ReminderDate.ToString(Resources.FormattedDate);
         string formattedTime = reminder.ReminderDate.ToString(Resources.FormattedTime);
         var userDetail = await _reminderRepository.GetUserById(reminder.CreatedBy);
         if (userDetail is null)
         {
            return false;
         }
         await _backgroundJobs.SendEmail(() => _mailService.SendEmail(new EmailModel
         {
            ToEmail = userDetail.EmailId,
            Subject = string.Format(Resources.ReminderSubject, reminder.Title),
            Body = string.Format(Resources.ReminderBody, userDetail.FirstName, reminder.Title, formattedDate, formattedTime) + Resources.Footer
         }));
         return true;
      }
   }
}
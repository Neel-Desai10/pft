using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Model.AuthoritySetting;
using Microsoft.Extensions.Logging;
using FinanceTracker.Hangfire.Interface;
using FinanceTracker.Utility.Model;
using FinanceTracker.DAL;
using FinanceTracker.BLL.Utility;
using FinanceTracker.Utility.Interface;
using Microsoft.Extensions.Options;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Utility.Enum;
namespace FinanceTracker.BLL.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IBackgroundJobs _backgroundJobs;
    private readonly IMailService _mailService;
    private readonly IAuthorityRegistrationServices _authorityRegistration;
    private readonly AuthorityModel _authoritySettings;
    private readonly INotificationService _notificationService;
    private readonly ILogger<UserServices> _logger;
    private readonly MailSettings _mailSettings;
    public UserServices(IUserRepository userRepository, IAuthorityRegistrationServices authorityRegistration, IOptions<MailSettings> mailSettings, IMailService mailService, ILogger<UserServices> logger, AuthorityModel authoritySettings, INotificationService notificationService, IBackgroundJobs backgroundJobs)
    {
        _userRepository = userRepository;
        _authorityRegistration = authorityRegistration;
        _notificationService = notificationService;
        _authoritySettings = authoritySettings;
        _backgroundJobs = backgroundJobs;
        _logger = logger;
        _mailSettings = mailSettings.Value;
        _mailService = mailService;
    }

    public Task<bool> IsEmailExist(string email)
    {
        return _userRepository.IsEmailAlreadyExist(email);
    }
    public async Task RegisterUserWithAuthority(UserRequestDto userRequest)
    {
        if (await IsEmailExist(userRequest.Email))
        {
            throw new Exception(ValidationResources.EmailAlreadyExist);
        }
        var userRegistration = userRequest.AuthorityUserMapper(_authoritySettings.ClientId);
        var response = await _authorityRegistration.RegisterUser(userRegistration);
        if (response.Response.IsSuccessful)
        {
            userRequest.SubjectId = response.Response.SubjectId;
            var data = await _userRepository.AddUser(userRequest);
            await _notificationService.AddNotification((int)data.UserId);
            await _backgroundJobs.SendEmail(() => _mailService.SendEmail(new EmailModel
            {
                ToEmail = _mailSettings.AdminMail,
                Subject = Resources.RegisterMailSubject + $"{data.FirstName}",
                Body = Resources.RegisterMailBody + $"{_mailSettings.LoginUrl}" + Resources.BreakLine + Resources.BreakLine + Resources.Footer
            }));
            response.Response.ResponseStatus = ValidationResources.RegistrationSuccess;
        }
        else
        {
            throw new Exception(ResponseResources.ErrorRegister);
        }
    }
    public async Task<UserDetailDto> GetUserBySubjectId(string subjectId)
    {
        var userDetail = await _userRepository.GetUser(subjectId);
        return userDetail;
    }
    public async Task<bool> IsSubjectIdExistService(string subjectId)
    {
        return await _userRepository.IsSubjectIdExist(subjectId);
    }
    public async Task<TransactionResponseDto> GetTotalTransactionsByYear(int userId, int year)
    {
        var transactions = await _userRepository.GetTransactionsByYear(userId, year);
        var transactionSummary = new TransactionResponseDto();

        transactionSummary.TotalTransactionData= transactions
            .GroupBy(t => t.TransactionDate.Month)
            .Select(g =>
            {
                var totalIncome = g.Where(t => t.Category.CategoryTypeId == (int)CategoryTypeEnum.Income).Sum(t => t.Amount);
                var totalExpenses = g.Where(t => t.Category.CategoryTypeId == (int)CategoryTypeEnum.Expense).Sum(t => t.Amount);

                return new TotalTransactionDto
                {
                    Month = g.Key,
                    TotalIncome = totalIncome,
                    TotalExpenses = totalExpenses,
                    AvailableBalance = totalIncome - totalExpenses
                };
            })
            .OrderBy(t => t.Month)
            .ToList();
        return transactionSummary;
    }


}
using System.Text;
using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.BLL.Utility;
using FinanceTracker.DAL;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Hangfire.Interface;
using FinanceTracker.Model.AuthoritySetting;
using FinanceTracker.Utility.Interface;
using FinanceTracker.Utility.Model;
using Microsoft.Extensions.Options;

namespace FinanceTracker.BLL.Services
{
    public class AdminCreateUserServices : IAdminCreateUserServices

    {
        private readonly IAdminCreateUserRepository _adminCreateUserRepository;
        private readonly IAuthorityRegistrationServices _authorityRegistration;
        private readonly AuthorityModel _authoritySettings;
        private readonly IBackgroundJobs _backgroundJobs;
        private readonly MailSettings _mailSettings;
        private readonly IMailService _mailService;

        public AdminCreateUserServices(IAdminCreateUserRepository adminCreateUserRepository, IAuthorityRegistrationServices authorityRegistration, IOptions<MailSettings> mailSettings, IMailService mailService, AuthorityModel authoritySettings, IBackgroundJobs backgroundJobs)
        {
            _adminCreateUserRepository = adminCreateUserRepository;
            _authorityRegistration = authorityRegistration;
            _authoritySettings = authoritySettings;
            _backgroundJobs = backgroundJobs;
            _mailSettings = mailSettings.Value;
            _mailService = mailService;
        }

        public Task<bool> IsEmailExist(string email)
        {
            return _adminCreateUserRepository.IsEmailAlreadyExist(email);
        }
        public async Task AdminCreateUser(CreateUserDto createUser,int loggedInUserId)
        {
            var randomPassword = GenerateRandomPassword();
            if(await IsEmailExist(createUser.Email))
            {
                throw new Exception(ValidationResources.EmailAlreadyExist);
            }
            AuthorityRegistrationDto userRegistration = AuthorityUserRegistration(createUser, randomPassword);
            var response = await _authorityRegistration.AdminRegisterUser(userRegistration);
            if (response.Response.IsSuccessful)
            {
                await UserRegistration(createUser, response, loggedInUserId);
                await _adminCreateUserRepository.CreateUserFromAdmin(createUser);
                await _backgroundJobs.SendEmail(() => _mailService.SendEmail(new EmailModel
                {
                    ToEmail = createUser.Email,
                    Subject = $" {createUser.FirstName}" + Resources.CreateUserMailSubject,
                    Body = Resources.CreateUserMailBody + Resources.CreateUserMailBodyUserCreds + $"{createUser.Email}" + Resources.PasswordString + $"{randomPassword}" + Resources.Note + $"{_mailSettings.LoginUrl}" + Resources.Footer
                }));
            }
            else
            {
                throw new Exception(ResponseResources.ErrorRegister);
            }
        }
        private async Task UserRegistration(CreateUserDto createUser, ResponseDto response,int loggedInUserId)
        {
            createUser.CreatedDate = DateTime.Now;
            createUser.CreatedBy = loggedInUserId;
            createUser.UserStatusId = (byte)UserStatusEnum.Approved;
            createUser.RoleId = (byte)RolesEnum.Employee;
            createUser.StatusTimestamp = DateTime.Now;
            createUser.IsActive = true;
            createUser.SubjectId = response.Response.SubjectId;
        }
        private AuthorityRegistrationDto AuthorityUserRegistration(CreateUserDto createUser, string randomPassword)
        {
            return new AuthorityRegistrationDto()
            {
                FullName = createUser.FirstName + createUser.LastName,
                Email = createUser.Email,
                UserName = createUser.Email,
                IsActive = true,
                PasswordHash = randomPassword,
                UserClient = _authoritySettings.ClientId,
                Claims = new List<ClaimsDto>(),
                Roles = new List<RoleDto>() { new RoleDto { Name = ValidationResources.Employee } }
            };
        }
        private static string GenerateRandomPassword()
        {
            Random random = new Random();
            int length = random.Next(8, 26);

            StringBuilder password = new StringBuilder();
            password.Append(ValidationResources.UppercaseChars[random.Next(ValidationResources.UppercaseChars.Length)]);
            password.Append(ValidationResources.LowercaseChars[random.Next(ValidationResources.LowercaseChars.Length)]);
            password.Append(ValidationResources.Digits[random.Next(ValidationResources.Digits.Length)]);
            password.Append(ValidationResources.SpecialChars[random.Next(ValidationResources.SpecialChars.Length)]);
            for (int i = 4; i < length; i++)
            {
                string possibleChars = ValidationResources.UppercaseChars + ValidationResources.LowercaseChars + ValidationResources.Digits + ValidationResources.SpecialChars;
                password.Append(possibleChars[random.Next(possibleChars.Length)]);
            }
            return new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());
        }
    }
}

using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.BLL.Utility;
using FinanceTracker.DAL;
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.DTO;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Hangfire.Interface;
using FinanceTracker.Utility.Enum;
using FinanceTracker.Utility.Interface;
using FinanceTracker.Utility.Model;
using Microsoft.Extensions.Options;

namespace FinanceTracker.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthorityRegistrationServices _authorityRegistration;
        private readonly IBackgroundJobs _backgroundJobs;
        private readonly ApplicationDbContext _userDbContext;
        private readonly MailSettings _mailSettings;
        private readonly IMailService _mailService;

        public AdminService(IAdminRepository adminRepository, IBackgroundJobs backgroundJobs, IAuthorityRegistrationServices authorityRegistration, IOptions<MailSettings> mailSettings, IMailService mailService, ApplicationDbContext userDbContext)
        {
            _adminRepository = adminRepository;
            _authorityRegistration = authorityRegistration;
            _backgroundJobs = backgroundJobs;
            _userDbContext = userDbContext;
            _mailSettings = mailSettings.Value;
            _mailService = mailService;
        }
        public async Task<UserListDto> GetUserDetailByStatus(UserStatusListEnum userStatusId, PaginationParameters paginationParams,string searchParam)
        {
            return await _adminRepository.GetUserDetail(userStatusId, paginationParams,searchParam);
        }
        public async Task<string> UpdateUserStatusId(List<ApprovalRequestDto> userStatusUpdates,int loggedInUserId)
        {
            string responseMessage ;
            int approvedUsers = 0;
            int rejectedUsers = 0;
            try
            {
                foreach (var userStatus in userStatusUpdates)
                {
                    var userDetails = await _adminRepository.GetUserById(userStatus.UserId);
                    if (userDetails is null)
                    {
                        throw new Exception(ValidationResources.InvalidUser);
                    }
                    if (userDetails.UserStatusId == (int)UserStatusEnum.Pending)
                    {
                        bool isSuccess = false;
                        switch (userStatus.UserStatusId)
                        {
                            case (int)UserStatusEnum.Approved:
                                isSuccess = await UpdateApproveUserStatus(userDetails,loggedInUserId);
                                if (isSuccess) approvedUsers++;
                                break;
                            case (int)UserStatusEnum.Rejected:
                                isSuccess = await UpdateRejectUserStatus(userDetails, userStatus.RejectionReason,loggedInUserId);
                                if (isSuccess) rejectedUsers++;
                                break;
                            default:
                                responseMessage = ValidationResources.InvalidStatus;
                                break;
                        }
                    }
                }
                responseMessage = await ResponseMessage(approvedUsers, rejectedUsers);
            }
            catch (Exception ex)
            {
                responseMessage = ValidationResources.ServerError + ex.Message;
            }
            return responseMessage;
        }
        private async Task<string> ResponseMessage(int approvedUsers, int rejectedUsers)
        {
            string responseMessage;
            if (approvedUsers > 0)
            {
                responseMessage = $"{(approvedUsers == 1 ? ValidationResources.UserIs : ValidationResources.SelectedUser)}{ValidationResources.Approved}";
            }
            else if (rejectedUsers > 0)
            {
                responseMessage = $"{(rejectedUsers == 1 ? ValidationResources.UserIs : ValidationResources.SelectedUser)}{ValidationResources.Rejected}";
            }
            else
            {
                responseMessage = ValidationResources.AlreadyUpdateStatus;
            }
            return responseMessage;
        }
        private async Task<bool> UpdateApproveUserStatus(UserModel userDetails, int? loggedInUserId)
        {
            userDetails.ModifiedDate = DateTime.Now;
            userDetails.ModifiedBy = loggedInUserId;
            userDetails.StatusTimestamp = DateTime.Now;
            userDetails.UserStatusId = (int)UserStatusEnum.Approved;
            userDetails.IsActive = true;
            var isAuthoritySuccess = await _authorityRegistration.UpdateApprove(userDetails.SubjectId);
            var isDbSuccess = await _adminRepository.UpdateUserStatus(userDetails);
            if (isDbSuccess && isAuthoritySuccess.IsSuccessful)
            {
                await _backgroundJobs.SendEmail(() => _mailService.SendEmail(new EmailModel
                {
                    ToEmail = userDetails.EmailId,
                    Subject = Resources.ApproveMailSubject,
                    Body = Resources.ApproveMailBody + $"{_mailSettings.LoginUrl}" + Resources.Footer
                }));
                return true;
            }
            return false;
        }
        private async Task<bool> UpdateRejectUserStatus(UserModel userDetails, string rejectionReason, int loggedInUserId)
        {
            userDetails.ModifiedDate = DateTime.Now;
            userDetails.ModifiedBy = loggedInUserId;
            userDetails.StatusTimestamp = DateTime.Now;
            userDetails.UserStatusId = (int)UserStatusEnum.Rejected;
            userDetails.RejectionReason = rejectionReason;
            var isDbSuccess = await _adminRepository.UpdateUserStatus(userDetails);
            if (isDbSuccess)
            {
                await _backgroundJobs.SendEmail(() => _mailService.SendEmail(new EmailModel
                {
                    ToEmail = userDetails.EmailId,
                    Subject = Resources.RejectionMailSubject,
                    Body = Resources.RejectionMailBody + $"{userDetails.RejectionReason}" + Resources.Footer
                }));
                return true;
            }
            return false;
        }

        public async Task<string> UpdateUserAccountStatus(int userId, bool isActive,int loggedInUserId)
        {
            var userData = await _adminRepository.GetUserById(userId);
            if (userData is not null)
            {
                userData.IsActive = isActive;
                userData.ModifiedDate = DateTime.Now;
                userData.ModifiedBy = loggedInUserId;
                var roleId = await _adminRepository.GetRoleByUserId(userId);
                if (roleId == (int)RolesEnum.Admin)
                {
                    throw new Exception(ValidationResources.AdminCanNotDeActive);
                }
                else
                {
                    var response = await _authorityRegistration.UpdateUserAccountStatus(userData.SubjectId, isActive);
                    if (response.IsSuccessful)
                    {
                        await _adminRepository.UpdateUserStatus(userData);
                        string action = isActive ? ResponseResources.AccountActivatedMessage : ResponseResources.AccountDeactivatedMessage;
                        return string.Format(ResponseResources.AccountStatusMessageTemplate, userData.FirstName, action);
                    }
                    else
                    {
                        throw new Exception(ValidationResources.AuthorityUpdateFailed);
                    }
                }
            }
            else
            {
                throw new Exception(ValidationResources.InvalidUser);
            }
        }

        public async Task<ActivityChartsResponseDto> GetActivityCharts(int year, int month)
        {
            var users = await _adminRepository.GetActivityCharts(year, month);
            return new ActivityChartsResponseDto()
            {
                PendingUsers = users.Count(x => x.UserStatusId == (int)UserStatusListEnum.Pending),
                ApprovedUsers = users.Count(x => x.UserStatusId == (int)UserStatusListEnum.Approved),
                RejectedUsers = users.Count(x => x.UserStatusId == (int)UserStatusListEnum.Rejected),
                RegisteredUsers = users.Count
            };
        }
    }
}
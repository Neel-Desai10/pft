using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos.AuthorityDto;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.Model.AuthoritySetting;
using FinanceTracker.DAL.Resources;
using RestSharp;

namespace FinanceTracker.BLL.Services
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IAuthorityRegistrationServices _authorityRegistration;
        public ChangePasswordService(IAuthorityRegistrationServices authorityRegistration)
        {
            _authorityRegistration = authorityRegistration;
        }
        public async Task<RestResponse> ChangePassword(string subjectId, ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
            {
                throw new Exception(ValidationResources.OldNewPasswordMustNotMatch);
            }
            var changePassword = new AuthorityChangePasswordDto()
            {
                OldPassword = changePasswordDto.OldPassword,
                NewPassword = changePasswordDto.NewPassword
            };
            return await _authorityRegistration.ChangePasswordWithAuthority(subjectId, changePassword);
        }
    }
}
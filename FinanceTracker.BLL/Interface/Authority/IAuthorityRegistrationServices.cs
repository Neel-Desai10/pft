using FinanceTracker.DAL;
using FinanceTracker.DAL.Dtos.AuthorityDto;
using FinanceTracker.DAL.Dtos;
using RestSharp;

namespace FinanceTracker.BLL
{
    public interface IAuthorityRegistrationServices
    {
        Task<ResponseDto> AdminRegisterUser(AuthorityRegistrationDto authorityRegisterModel);
        Task<ResponseDto> RegisterUser(AuthorityRegistrationDto authorityRegisterModel);
        Task<RestResponse> UpdateApprove(string subjectId);
        Task<RestResponse> UpdateUserAuthority(string subjectId,AuthorityUpdateDto authorityUpdateDto);
        Task<RestResponse> UpdateUserAccountStatus(string subjectId, bool isActive);
        Task<RestResponse> ChangePasswordWithAuthority(string subjectId, AuthorityChangePasswordDto authorityChangePasswordDto);
    
    }
}
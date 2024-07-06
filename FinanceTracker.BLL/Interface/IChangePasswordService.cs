using FinanceTracker.DAL.Dtos.UserDto;
using RestSharp;

namespace FinanceTracker.BLL.Interface
{
    public interface IChangePasswordService
    {
        Task<RestResponse> ChangePassword(string subjectId,ChangePasswordDto changePasswordDto);
    }
}
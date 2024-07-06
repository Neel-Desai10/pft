using FinanceTracker.DAL.Dtos.AuthorityDto;
using FinanceTracker.DAL.Model.AuthoritySetting;
using RestSharp;

namespace FinanceTracker.DAL
{
    public interface IRestServiceClientRepository
    {
        Task<UserResponseModel> InvokePostAsync<T>(string requestUri, T model, string token = null)  where T : class;
        Task<RestResponse> InvokePutAsync<T>(string requestUri, string subjectId,bool isActive, string? token = null)  where T : class;
        Task<RestResponse> InvokePutPasswordAsync(string requestUri,string subjectId , AuthorityChangePasswordDto authorityChangePasswordDto,string token) ;
        Task<RestResponse> InvokeUpdateAsync<T>(string requestUri, string subjectId, T model, string? token = null)  where T : class;
    }
}
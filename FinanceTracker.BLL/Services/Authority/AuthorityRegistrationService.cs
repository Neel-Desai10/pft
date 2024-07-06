using FinanceTracker.BLL;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AuthorityDto;
using FinanceTracker.DAL.Interface.TokenRepository;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Model.AuthoritySetting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace FinanceTracker.DAL
{
    public class AuthorityRegistrationServices : IAuthorityRegistrationServices
    {
        private readonly IRestServiceClientRepository _restServiceClient;

        private readonly ITokenRepository _tokenRepository;

        private readonly AuthorityModel _authoritySettings;

        private readonly ILogger<AuthorityRegistrationServices> _logger;

        public AuthorityRegistrationServices(ITokenRepository tokenRepository, IRestServiceClientRepository restServiceClient, AuthorityModel authoritySettings, ILogger<AuthorityRegistrationServices> logger)
        {
            _tokenRepository = tokenRepository;
            _restServiceClient = restServiceClient;
            _authoritySettings = authoritySettings;
            _logger = logger;
        }

        public virtual async Task<ResponseDto> AdminRegisterUser(AuthorityRegistrationDto authorityRegisterModel)
        {
            string baseUri = $"{_authoritySettings.BaseUri}" + ValidationResources.RegisterUrl;
            authorityRegisterModel.UserClient = _authoritySettings.ClientId;

            var token = await _tokenRepository.GenerateTokenAsync();
            var response = await _restServiceClient.InvokePostAsync<AuthorityRegistrationDto>(baseUri, authorityRegisterModel, token);

            response.ResponseStatus = token;
            dynamic data = JsonConvert.DeserializeObject(response.SubjectId);
            response.SubjectId = data[ValidationResources.SubjectId];

            var Response = new ResponseDto { Response = response };
            return Response;
        }

        public virtual async Task<ResponseDto> RegisterUser(AuthorityRegistrationDto authorityRegistration)
        {
            var response = await AdminRegisterUser(authorityRegistration);
            bool isActive = false;
            await UpdateUserStatus(response.Response.SubjectId, isActive, response.Response.ResponseStatus);
            return response;
        }
        public async Task<RestResponse> UpdateApprove(string subjectId)
        {
            var token = await _tokenRepository.GenerateTokenAsync();
            var uri = $"{_authoritySettings.BaseUri}" + ResponseResources.PutApiUrl + $"{subjectId}";
            bool isActive = true;
            var response = await _restServiceClient.InvokePutAsync<string>(uri, subjectId, isActive, token);
            return response;
        }
        private async Task<RestResponse> UpdateUserStatus(string subjectId, bool isActive, string token)
        {
            var uri = $"{_authoritySettings.BaseUri}" + ResponseResources.PutApiUrl + $"{subjectId}";
            var response = await _restServiceClient.InvokePutAsync<string>(uri, subjectId, isActive, token);
            return response;
        }

        public async Task<RestResponse> UpdateUserAccountStatus(string subjectId, bool isActive)
        {
            var token = await _tokenRepository.GenerateTokenAsync();
            var uri = $"{_authoritySettings.BaseUri}" + ResponseResources.PutApiUrl + $"{subjectId}";
            var response = await _restServiceClient.InvokePutAsync<string>(uri, subjectId, isActive, token);
            return response;
        }
        public async Task<RestResponse> ChangePasswordWithAuthority(string subjectId, AuthorityChangePasswordDto authorityChangePasswordDto)
        {
            try
            {
                var token = await _tokenRepository.GenerateTokenAsync();
                string baseUri = $"{_authoritySettings.BaseUri}" + ResponseResources.ChangePasswordUrl + $"{subjectId}";
                var response = await _restServiceClient.InvokePutPasswordAsync(baseUri, subjectId, authorityChangePasswordDto, token);

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw new Exception(ex.Message);
            }
        }

        public async Task<RestResponse> UpdateUserAuthority(string subjectId, AuthorityUpdateDto authorityUpdateDto)
        {
            var tokens = await _tokenRepository.GenerateTokenAsync();
            var uri = $"{_authoritySettings.BaseUri}" + ResponseResources.UpdateApiUrl + $"{subjectId}";
            var response = await _restServiceClient.InvokeUpdateAsync<AuthorityUpdateDto>(uri, subjectId, authorityUpdateDto, tokens);
            return response;
        }
    }
}
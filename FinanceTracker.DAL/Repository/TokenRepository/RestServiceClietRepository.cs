using FinanceTracker.DAL.Dtos.AuthorityDto;
using FinanceTracker.DAL.Mapper;
using FinanceTracker.DAL.Model.AuthoritySetting;
using FinanceTracker.DAL.Resources;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace FinanceTracker.DAL
{
    public class RestServiceClientRepository : IRestServiceClientRepository
    {
        private readonly ILogger<RestServiceClientRepository> _logger;
        public RestServiceClientRepository(ILogger<RestServiceClientRepository> logger)
        {
            _logger = logger;
        }
        public async Task<UserResponseModel> InvokePostAsync<T>(string requestUri, T model, string? token = null) where T : class
        {
            var client = new RestSharp.RestClient(requestUri);

            var restRequest = new RestSharp.RestRequest(requestUri, Method.Post)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = ValidationResources.ContentType; }
            };
            restRequest.AddJsonBody(model);
            if (!string.IsNullOrWhiteSpace(token))
            {
                restRequest.AddParameter(ValidationResources.AddParameter, token, ParameterType.HttpHeader);
            }
            RestResponse<UserResponseModel> response = client.Execute<UserResponseModel>(restRequest);

            _logger.LogInformation(response.IsSuccessful ? ValidationResources.AuthorityPostSuccess : ValidationResources.AuthorityPostFailed,
                           requestUri, response.StatusCode, response.Content);
            return response.UserResponse();
        }
        public async Task<RestResponse> InvokePutAsync<T>(string requestUri, string subjectId, bool isActive, string? token = null) where T : class
        {
            var client = new RestSharp.RestClient(requestUri);

            var restRequest = new RestSharp.RestRequest(requestUri, Method.Put)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = ValidationResources.ContentType; }
            };
            restRequest.AddUrlSegment(ValidationResources.QueryParameter, subjectId);
            restRequest.AddQueryParameter(ValidationResources.IsEnabled, isActive);
            if (!string.IsNullOrWhiteSpace(token))
            {
                restRequest.AddParameter(ValidationResources.AddParameter, token, ParameterType.HttpHeader);
            }

            RestResponse response = client.Execute(restRequest);

            _logger.LogInformation(response.IsSuccessful ? ValidationResources.AuthoritySuccessLog : ValidationResources.AuthorityFailedLog,
                           requestUri, response.StatusCode, response.Content);
            return response;
        }
        public async Task<RestResponse> InvokePutPasswordAsync(string requestUri, string subjectId, AuthorityChangePasswordDto authorityChangePasswordDto, string? token = null)
        {
            var client = new RestSharp.RestClient(requestUri);

            var restRequest = new RestSharp.RestRequest(requestUri, Method.Put)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = ValidationResources.ContentType; }
            };
            restRequest.AddQueryParameter(ValidationResources.QueryParameter, subjectId);
            restRequest.AddJsonBody(authorityChangePasswordDto);
            if (!string.IsNullOrWhiteSpace(token))
            {
                restRequest.AddParameter(ValidationResources.AddParameter, token, ParameterType.HttpHeader);
            }
            RestResponse response = client.Execute(restRequest);

            _logger.LogInformation(response.IsSuccessful ? ValidationResources.AuthoritySuccessMessage : ValidationResources.AuthorityFailureMessage,
                           requestUri, response.StatusCode, response.Content);
            return response;
        }
        public async Task<RestResponse> InvokeUpdateAsync<T>(string requestUri, string subjectId, T model, string? token = null) where T : class
        {
            var client = new RestSharp.RestClient(requestUri);

            var restRequest = new RestSharp.RestRequest(requestUri, Method.Put)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = ValidationResources.ContentType; }
            };

            restRequest.AddQueryParameter(ValidationResources.QueryParameter, subjectId);
            restRequest.AddJsonBody(model);
            if (!string.IsNullOrWhiteSpace(token))
            {
                restRequest.AddParameter(ValidationResources.AddParameter, token, ParameterType.HttpHeader);
            }

            RestResponse response = client.Execute(restRequest);

            _logger.LogInformation(response.IsSuccessful ? ValidationResources.AuthoritySuccessMessage : ValidationResources.AuthorityFailureMessage,
                           requestUri, response.StatusCode, response.Content);
            return response;
        }
    }
}
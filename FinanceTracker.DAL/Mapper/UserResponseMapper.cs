using FinanceTracker.DAL.Model.AuthoritySetting;
using FinanceTracker.DAL.Resources;
using RestSharp;

namespace FinanceTracker.DAL.Mapper
{
    public static class UserResponseMapper
    {
        public static UserResponseModel UserResponse(this RestResponse<UserResponseModel> users)
        {
            if (users.IsSuccessful)
            {
                return new UserResponseModel
                {
                    StatusCode = (int)users.StatusCode,
                    IsSuccessful = users.IsSuccessful,
                    ResponseStatus = ResponseResources.SuccessMessage,
                    SubjectId = users.Content
                };
            }
            else
            {
                var error = new List<string> { ResponseResources.FailedMessage };
                return new UserResponseModel
                {
                    StatusCode = (int)users.StatusCode,
                    ErrorMessage = error
                };
            }
        }
    }
}
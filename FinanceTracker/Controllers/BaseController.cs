using System.IdentityModel.Tokens.Jwt;
using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility;

namespace FinanceTracker.Controllers;
public class BaseController : BaseResponse
{
    private readonly IUserDetailsService _userDetailsService;
    public BaseController(IUserDetailsService userDetailsService)
    {
        _userDetailsService = userDetailsService;
    }
    protected int GetUserId()
    {
        var data = _userDetailsService.GetUserIdBySubjectId(GetSubjectId());
        if (data is null)
        {
            throw new Exception(ResponseResources.NoUserFound);
        }
        if (!data.IsActive)
        {
            throw new Exception(ResponseResources.IsActiveMessage);
        }
        return data.LoggedInUserId;
    }
    protected string GetSubjectId()
    {
        return GetValue(ValidationResources.Subject);
    }
    private string GetValue(string field)
    {
        JwtSecurityToken token = GetTokenDetails();
        string data = token.Claims.FirstOrDefault(x => x.Type == field).Value.ToString();
        return data;
    }
    private JwtSecurityToken GetTokenDetails()
    {
        string jwtToken = HttpContext.Request.Headers[ValidationResources.Authorization];
        if (jwtToken is null)
        {
            throw new Exception(ValidationResources.NullToken);
        }
        jwtToken = jwtToken.Replace(ValidationResources.Bearer, ValidationResources.EmptyString);
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(jwtToken);
        return token;
    }
}
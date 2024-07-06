using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.DTOs;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.DAL;

public static class UserDTOmapper
{
    public static UserModel signupDTOMapper(this UserRequestDto users)
    {
        return new UserModel{
            UserId = users.UserId,
            FirstName = users.FirstName,
            LastName = users.LastName,
            EmailId = users.Email,
            PhoneNumber = users.PhoneNumber,
            SubjectId = users.SubjectId
        };
    }
    public static UserDetailDto UserDetailDTOMapper(this UserModel user)
    {
        return new UserDetailDto{
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailId = user.EmailId,
            PhoneNumber = user.PhoneNumber,
            UserStatusId = user.UserStatusId,
            RoleId = user.RoleId,
            SubjectId = user.SubjectId,
            GenderId = user.GenderId,
            CountryId = user.CountryId,
            StateId = user.StateId,
            CityId = user.CityId,
            Pincode = user.Pincode,
            DateOfBirth = user.DateOfBirth,
            Image = user.Image,
            Address = user.Address,
        };
    }
    public static AuthorityRegistrationDto AuthorityUserMapper(this UserRequestDto userRequest, string ClientId)
    {
        return new AuthorityRegistrationDto()
        {
            FullName = userRequest.FirstName + userRequest.LastName,
            Email = userRequest.Email,
            UserName = userRequest.Email,
            PasswordHash = userRequest.PasswordHash,
            UserClient = ClientId,
            Claims = new List<ClaimsDto>(),
            Roles = new List<RoleDto>()
            {
                new RoleDto { Name= ValidationResources.Employee}
            }
        };
    }
}

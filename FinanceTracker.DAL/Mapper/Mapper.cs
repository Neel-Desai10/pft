using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.DTOs;

namespace FinanceTracker.DAL.Dtos
{
    public static class Mapper
    {
        public static UserModel CreateUsersDtoMapper(this CreateUserDto createUser)
        {
            return new UserModel
            {
                UserId = createUser.UserId,
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                EmailId = createUser.Email,
                CreatedDate = createUser.CreatedDate,
                CreatedBy = createUser.CreatedBy,
                StatusTimestamp = createUser.StatusTimestamp,
                UserStatusId = createUser.UserStatusId,
                RoleId = createUser.RoleId,
                IsActive = createUser.IsActive,
                SubjectId = createUser.SubjectId
            };
        }

        public static void UpdateUserMethod(EditUserDto editUserDto, AuthorityUpdateDto authorityUpdate, UserModel user, int loggedInUserId)
        {
            user.LastName = editUserDto.LastName;
            user.GenderId = (byte?)editUserDto.GenderId;
            user.DateOfBirth = editUserDto.DateOfBirth;
            user.PhoneNumber = editUserDto.PhoneNumber;
            user.Address = editUserDto.Address;
            user.Image = editUserDto.Image;
            user.ModifiedDate = editUserDto.ModifiedDate;
            user.ModifiedBy = loggedInUserId;
            user.CountryId = editUserDto.CountryId;
            user.StateId = editUserDto.StateId;
            user.CityId = editUserDto.CityId;
            user.Pincode = editUserDto.Pincode;
            authorityUpdate.FullName = user.FirstName + user.LastName;
            authorityUpdate.Email = user.EmailId;
            authorityUpdate.PhoneNumber = user.PhoneNumber;
            authorityUpdate.Claims = new List<ClaimsDto>();
        }
    }
}
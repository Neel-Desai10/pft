using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _userContext;
        public UserDetailsRepository(ApplicationDbContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<UserDetailsResponseDto> GetUserDetails(int userId)
        {
            var userDetailsResponse = new UserDetailsResponseDto();
            var userData = await _userContext.UserData.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userData != null)
            {
                return userDetailsResponse = new UserDetailsResponseDto
                {
                    UserId = userData.UserId,
                    UserName = userData.FirstName + ResponseResources.BlankSpace + userData.LastName,
                    PhoneNumber = userData.PhoneNumber,
                    DateOfBirth = userData.DateOfBirth,
                    Gender = userData.GenderId != null ? await _userContext.Gender.Where(x => x.GenderId == userData.GenderId).Select(x => x.Gender).FirstOrDefaultAsync() : null,
                    Address = userData.Address,
                    State = userData.StateId != null ? await _userContext.States.Where(x => x.StateId == userData.StateId).Select(x => x.State).FirstOrDefaultAsync() : null,
                    City = userData.CityId != null ? await _userContext.Cities.Where(x => x.CityId == userData.CityId).Select(x => x.City).FirstOrDefaultAsync() : null,
                    Country = userData.CountryId != null ? await _userContext.Countries.Where(x => x.CountryId == userData.CountryId).Select(x => x.Country).FirstOrDefaultAsync() : null,
                    Pincode = userData.Pincode,
                };
            }
            else
            {
                return userDetailsResponse;
            }
        }
        public UserResponseDto GetUserIdBySubjectId(string subjectId)
        {
            var data =  _userContext.UserData.Where(x => x.SubjectId == subjectId)
                .Select(x => new UserResponseDto{
                    LoggedInUserId = (int)x.UserId,
                    IsActive = (bool)x.IsActive
                }).FirstOrDefault();
            return data;
        }
    }
}
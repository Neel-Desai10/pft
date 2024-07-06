using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.BLL.Services
{
    public class EditUserService : IEditUserService
    {
        private readonly IAuthorityRegistrationServices _authorityRegistration;
        private readonly IEditUserRepository _editUserRepository;

        public EditUserService(IEditUserRepository editUserRepository, IAuthorityRegistrationServices authorityRegistration)
        {
            _editUserRepository = editUserRepository;
            _authorityRegistration = authorityRegistration;
        }
        public async Task EditUsers(int userId, EditUserDto editUserDto, int loggedInUserId)
        {
            AuthorityUpdateDto authorityUpdate = new AuthorityUpdateDto();
            var user = await _editUserRepository.GetUserById(userId);
            if (user is not null)
            {
                Mapper.UpdateUserMethod(editUserDto, authorityUpdate, user, loggedInUserId);
            }
            else
            {
                throw new Exception(ValidationResources.InvalidUserID);
            }
            var response = await _authorityRegistration.UpdateUserAuthority(user.SubjectId, authorityUpdate);
            if(response.IsSuccessful){
            await _editUserRepository.EditUser(user);
            }else{
                throw new Exception(ValidationResources.UpdateAuthority);
            }
        }
    }
}
namespace FinanceTracker.DAL.Dtos.AdminDto
{
    public class UserListDto
    {
        public List<UserDto> UsersList { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
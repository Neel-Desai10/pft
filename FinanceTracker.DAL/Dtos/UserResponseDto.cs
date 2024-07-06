namespace FinanceTracker.DAL.Dtos
{
    public class UserResponseDto
    {
        public int LoggedInUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
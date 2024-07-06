namespace FinanceTracker.DAL.Dtos
{
    public class UserStatusCountResponseDto
    {
        public int PendingUsers { get; set; }
        public int ApprovedUsers { get; set; }
        public int RejectedUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InActiveUsers { get; set; }
    }
}
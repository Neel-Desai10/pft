namespace FinanceTracker.DAL.Dtos.AdminDto
{
    public class ActivityChartsResponseDto
    {
        public int PendingUsers { get; set; }
        public int ApprovedUsers { get; set; }
        public int RejectedUsers { get; set; }
        public int RegisteredUsers { get; set; }
    }
}
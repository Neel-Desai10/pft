namespace FinanceTracker.DAL.Dtos.AdminDto
{
    public class UserDto
    {
        public int? UserId { get; set;} 
        public DateTime? ModifiedDate { get; set; } 
        public DateTime? StatusTimestamp { get; set; } 
        public string FirstName { get; set; }
        public string? LastName { get; set; } 
        public string EmailId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? isActive { get; set; } 
        public string? RejectionReason { get; set; } 
        public byte? UserStatusId { get; set; }
    }
}
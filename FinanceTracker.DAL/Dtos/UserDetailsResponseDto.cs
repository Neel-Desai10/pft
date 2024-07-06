namespace FinanceTracker.DAL.Dtos
{
    public class UserDetailsResponseDto
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }
    }
}
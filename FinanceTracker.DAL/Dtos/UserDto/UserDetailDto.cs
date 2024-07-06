namespace FinanceTracker.DAL.Dtos.UserDto
{
    public class UserDetailDto
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailId { get; set; }
        public string? PhoneNumber { get; set; }
        public byte? UserStatusId { get; set; }
        public byte RoleId { get; set; }
        public string SubjectId { get; set; }
        public byte? GenderId { get; set; }
        public byte? CountryId { get; set; }
        public byte? StateId { get; set; }
        public int? CityId { get; set; }
        public string? Pincode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
    }
}
using FinanceTracker.DAL.DTOs;

namespace FinanceTracker.DAL.Dtos
{
    public class AuthorityUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<ClaimsDto> Claims { get; set; }
    }
}
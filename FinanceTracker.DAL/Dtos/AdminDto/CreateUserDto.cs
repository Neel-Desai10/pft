using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.DAL.Dtos.AdminDto;

public class CreateUserDto
{
    [JsonIgnore]
    public int? UserId { get; set;}
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ValidationResources.FirstNameRegexMessage)]
    [StringLength(30, MinimumLength = 2, ErrorMessage = ValidationResources.FirstNameLengthMessage)]
    public string FirstName { get; set; }
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ValidationResources.LastNameRegexMessage)]
    [StringLength(30, ErrorMessage = ValidationResources.LastNameLengthMessage)]
    public string? LastName { get; set; }
    [RegularExpression(@"^[a-zA-Z][\w\.-]*@(?:[a-zA-Z\d-]+\.)+(?:com|in)$", ErrorMessage = ValidationResources.InvalidEmail)]
    [StringLength(50, MinimumLength = 1, ErrorMessage = ValidationResources.EmailLength)]
    public string? Email { get; set; }
    [JsonIgnore]
    public byte? UserStatusId { get; set; }
    [JsonIgnore]
    public byte RoleId { get; set; }
    [JsonIgnore]
    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public int CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime? StatusTimestamp { get; set; }
    [JsonIgnore]
    public bool? IsActive { get; set; }
    [JsonIgnore]
    public string? SubjectId { get; set; }
}

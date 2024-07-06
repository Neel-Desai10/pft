using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FinanceTracker.DAL.Resources;
using FinanceTracker.DAL.Utility.Enum;

namespace FinanceTracker.DAL.Dtos
{
    public class EditUserDto
    {
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ValidationResources.LastNameMessage)]
        [StringLength(30, ErrorMessage = ValidationResources.LastNamesMessage)]
        public string? LastName { get; set; }
        [EnumDataType(typeof(GenderType))]
        public GenderType? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = ValidationResources.InvalidPhone)] 
        public string? PhoneNumber { get; set; }
        [StringLength(250, ErrorMessage = ValidationResources.AddressMessage)]
        public string? Address { get; set; }
        public string? Image { get; set; }
        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        public byte? CountryId { get; set; }
        public byte? StateId { get; set; }
        public int? CityId { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = ValidationResources.InvalidPincode)]
        public string? Pincode { get; set; }
    }
}
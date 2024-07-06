using System.ComponentModel.DataAnnotations;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.DAL.Dtos.UserDto
{
    public class ChangePasswordDto
    {
        [StringLength(25, MinimumLength = 8, ErrorMessage = ValidationResources.PasswordLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = ValidationResources.InvalidPassword)]
        public string OldPassword { get; set; }
        [StringLength(25, MinimumLength = 8, ErrorMessage = ValidationResources.PasswordLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = ValidationResources.InvalidPassword)]
        public  string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = ValidationResources.ConfirmPasswordValidation)]
        public string ConfirmPassword { get; set; }
    }
}
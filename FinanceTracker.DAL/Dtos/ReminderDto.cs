using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.DAL.Dtos
{
    public class ReminderDto
    {
        [Required(ErrorMessage = ValidationResources.RequiredFieldValidatingMessage)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ValidationResources.TitleLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationResources.RequiredFieldValidatingMessage )]
        [RegularExpression(@"^\d{1,8}(\.\d{1,2})?$", ErrorMessage = ValidationResources.AmountRegexMessage)]
        public decimal Value { get; set; }
        public DateTime ReminderDate { get; set; }
        public byte ReminderAlertId { get; set; } 
        [StringLength(100, ErrorMessage = ValidationResources.NoteLength)]
        public string? Notes { get; set; }
    }
}
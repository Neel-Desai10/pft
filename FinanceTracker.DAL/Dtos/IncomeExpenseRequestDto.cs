using System.ComponentModel.DataAnnotations;
using FinanceTracker.DAL.Resources;

namespace FinanceTracker.DAL.Dtos
{
    public class IncomeExpenseRequestDto
    {
        public byte CategoryId { get; set; }
        [RegularExpression(@"^\d{1,8}(\.\d{1,2})?$", ErrorMessage = ValidationResources.AmountRegexMessage)]   
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        [RegularExpression("^(?=.*[a-zA-Z0-9])[a-zA-Z0-9 ]+$", ErrorMessage = ValidationResources.DescriptionRegexMessage)]
        [StringLength(30, MinimumLength = 0, ErrorMessage = ValidationResources.DescriptionLengthMessage)]
        public string? Description { get; set; }
    }
}
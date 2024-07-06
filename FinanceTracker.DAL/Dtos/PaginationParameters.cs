using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DAL.Dtos
{
    public class PaginationParameters
    {
        [Required(ErrorMessage = "Page number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}
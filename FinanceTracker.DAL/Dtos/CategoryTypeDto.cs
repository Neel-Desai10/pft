using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DAL.Dtos
{
    public class CategoryTypeDto
    {
        [Key]
        public byte CategoryTypeId { get; set; }
        public string CategoryType { get; set; } 
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.DAL.Model
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }
        public short CategoryId { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
         [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
    }
}
namespace FinanceTracker.DAL.Dtos
{
    public class TotalTransactionResponseDto
    {
        public byte CategoryTypeId { get; set; } 
        public short CategoryId { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalCount { get; set; }
    }
}
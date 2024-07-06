namespace FinanceTracker.DAL.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public short CategoryId{ get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }
}
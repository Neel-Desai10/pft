namespace FinanceTracker.DAL.Dtos
{
    public class TransactionSummaryResponseDto
    {
        public List<TotalTransactionResponseDto> TotalTransactionData { get; set; } = new List<TotalTransactionResponseDto>();
        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
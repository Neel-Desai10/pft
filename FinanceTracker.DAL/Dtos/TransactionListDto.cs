namespace FinanceTracker.DAL.Dtos
{
    public class TransactionListDto
    {
        public List<UserTransactionDto> TransactionsDetail { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
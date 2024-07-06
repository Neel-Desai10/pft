namespace FinanceTracker.DAL.Dtos
{
    public class IncomeExpenseDto
    {
        public List<TransactionDto> IncomeExpenseDetails { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
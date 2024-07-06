namespace FinanceTracker.DAL.Dtos
{
    public class TotalTransactionDto
    {
        public int Month { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal AvailableBalance { get; set; }
    }
    }

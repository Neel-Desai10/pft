using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Mapper
{
    public static class TrasnsactionMapper
    {
        public static TransactionModel IncomeExpenseDtoMapper(this IncomeExpenseRequestDto incomeExpense)
        {
            return new TransactionModel
            {
                CategoryId = incomeExpense.CategoryId,
                Amount = incomeExpense.Amount,
                TransactionDate = incomeExpense.TransactionDate,
                Description = incomeExpense.Description
            };
        }
    }
}
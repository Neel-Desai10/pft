using CSharpFunctionalExtensions;
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Mapper;
using FinanceTracker.DAL.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.DAL.Repository
{
    public class IncomeExpenseRepository : IIncomeExpenseRepository
    {
        private readonly ApplicationDbContext _userContext;
        private readonly ILogger<IncomeExpenseRepository> _logger;
        public IncomeExpenseRepository(ApplicationDbContext userContext, ILogger<IncomeExpenseRepository> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }
        public async Task<int> GetTotalUsersCount(int userId, byte categoryTypeId, int month, int year)
        {
            int count = 0;
            var data = await _userContext.Transactions
                .Where(c => c.CreatedBy == userId && c.DeletedDate == null && c.TransactionDate.Month == month && c.TransactionDate.Year == year)
                .ToListAsync();
            foreach (var transactions in data)
            {
                var categoryType = await _userContext.Categories
                .Where(c => c.CategoryId == transactions.CategoryId)
                .Select(c => c.CategoryTypeId)
                .FirstOrDefaultAsync();
                if (categoryType != null && categoryType == categoryTypeId)
                {
                    count++;
                }
            }
            return count;
        }
        public async Task<List<TransactionDto>> GetTransactionsByCategoryType(int userId, byte categoryTypeId, PaginationParameters paginationParameters, int month, int year)
        {
            var data = await _userContext.Transactions
                .Where(x => x.CreatedBy == userId && x.DeletedDate == null && x.Category.CategoryTypeId == categoryTypeId && x.TransactionDate.Month == month && x.TransactionDate.Year == year)
                .OrderByDescending(x => x.TransactionDate)
                .ThenByDescending(x => x.CreatedDate)
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .Select(x => new TransactionDto
                {
                    TransactionId = x.TransactionId,
                    CategoryId = x.Category.CategoryId,
                    Category = x.Category.Category,
                    Description = x.Description,
                    TransactionDate = x.TransactionDate,
                    Amount = x.Amount
                })
                .ToListAsync();
            return data;
        }
        public async Task<List<UserTransactionDto>> UserTransactions(int userId, PaginationParameters paginationParameters, int month, int year)
        {
            var userTransactions = await _userContext.Transactions
               .Where(x => x.CreatedBy == userId && x.DeletedDate == null && x.TransactionDate.Month == month && x.TransactionDate.Year == year)
               .OrderByDescending(x => x.TransactionDate)
               .ThenByDescending(x => x.CreatedDate)
               .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
               .Take(paginationParameters.PageSize)
               .Select(x => new UserTransactionDto
               {
                   TransactionId = x.TransactionId,
                   Description = x.Description,
                   TransactionDate = x.TransactionDate,
                   Amount = x.Amount,
                   Category = x.Category.Category,
                   CategoryId = x.CategoryId,
                   CategoryTypeId = x.Category.CategoryTypeId,
               })
               .ToListAsync();
            return userTransactions;
        }
        public async Task<bool> CheckCategoryByCategoryId(int category, int categoryType)
        {
            return await _userContext.Categories.Where(c => c.CategoryId == category)
                                                .AnyAsync(c => c.CategoryTypeId == categoryType);
        }
        public async Task AddIncomeExpense(IncomeExpenseRequestDto incomeExpenseRequestDto, int userId)
        {
            var data = incomeExpenseRequestDto.IncomeExpenseDtoMapper();
            data.CreatedBy = userId;
            await _userContext.AddAsync(data);
            await _userContext.SaveChangesAsync();
        }
        public async Task<int> GetTotalUserTransaction(int userId, int month, int year)
        {
            return await _userContext.Transactions.Where(x => x.CreatedBy == userId && x.DeletedDate == null && x.TransactionDate.Month == month && x.TransactionDate.Year == year).CountAsync();
        }
        public async Task<List<UserTransactionDto>> UserTransactionData(int userId, int month, int year)
        {
            var userTransactions = await _userContext.Transactions
               .Where(x => x.CreatedBy == userId && x.TransactionDate.Month == month && x.TransactionDate.Year == year && x.DeletedDate == null)
               .OrderBy(x => x.TransactionDate)
               .ThenBy(x => x.CreatedDate)
               .Select(x => new UserTransactionDto
               {
                   TransactionId = x.TransactionId,
                   Description = x.Description,
                   TransactionDate = x.TransactionDate,
                   Amount = x.Amount,
                   Category = x.Category.Category,
                   CategoryId = x.CategoryId,
                   CategoryTypeId = x.Category.CategoryTypeId,
               })
               .ToListAsync();
            return userTransactions;
        }
        public async Task<TransactionModel> GetTransactionById(int userId,int transactionId)
        {
            var incomeExpenseData = await _userContext.Transactions.FirstOrDefaultAsync(x=>x.CreatedBy==userId &&  x.DeletedDate == null && x.TransactionId == transactionId);
            return incomeExpenseData;
        }
        public async Task DeleteIncomeExpense(TransactionModel transactionModel)
        {
             _userContext.Update(transactionModel);
            await _userContext.SaveChangesAsync();
        }

        public async Task<TransactionModel> GetIncomeExpenseById(int transactionId, int userId)
        {
            var incomeExpenseUpdateData = await _userContext.Transactions.FirstOrDefaultAsync(x => x.TransactionId == transactionId && x.DeletedDate == null);
            if (incomeExpenseUpdateData != null && incomeExpenseUpdateData.CreatedBy == userId)
            {
                return incomeExpenseUpdateData;
            }
            return null;
        }
        public async Task UpdateIncomeExpense(TransactionModel transactionModel)
        {
            _userContext.Update(transactionModel);
            await _userContext.SaveChangesAsync();
        }
    }
}
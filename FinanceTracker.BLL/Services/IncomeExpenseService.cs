using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using FinanceTracker.DAL.Resources;
using FinanceTracker.DAL.Utility.Enum;
using Microsoft.Extensions.Options;

namespace FinanceTracker.BLL.Services
{
    public class IncomeExpenseService : IIncomeExpenseService
    {
        private readonly IIncomeExpenseRepository _incomeExpenseRepository;
        private readonly ImagePathModel _imagePath;
        public IncomeExpenseService(IIncomeExpenseRepository incomeExpenseRepository, IOptions<ImagePathModel> imagePath)
        {
            _incomeExpenseRepository = incomeExpenseRepository;
            _imagePath = imagePath.Value;
        }
        public async Task<IncomeExpenseDto> GetTransactionsByCategoryType(int userId, byte categoryTypeId, PaginationParameters paginationParameters, int month, int year)
        {
            if (userId <= 0)
            {
                throw new Exception(ValidationResources.InvalidUserId);
            }
            if (paginationParameters.PageNumber <= 0 || paginationParameters.PageSize <= 0)
            {
                throw new Exception(ValidationResources.PaginationValidation);
            }
            var response = new IncomeExpenseDto();
            var incomeExpenseDetails = await _incomeExpenseRepository.GetTransactionsByCategoryType(userId, categoryTypeId, paginationParameters, month, year);
            response.IncomeExpenseDetails = incomeExpenseDetails;
            var totalRecordCount = await _incomeExpenseRepository.GetTotalUsersCount(userId, categoryTypeId, month, year);
            response.TotalRecordCount = totalRecordCount;
            return response;
        }
        public async Task<TransactionListDto> TransactionResponse(int userId, PaginationParameters paginationParameters, int month, int year)
        {
            if (userId <= 0)
            {
                throw new Exception(ValidationResources.InvalidUserId);
            }
            if (paginationParameters.PageNumber <= 0 || paginationParameters.PageSize <= 0)
            {
                throw new Exception(ValidationResources.PaginationValidation);
            }
            var responseData = new TransactionListDto();
            var data = await _incomeExpenseRepository.UserTransactions(userId, paginationParameters, month, year);
            var totalCount = await _incomeExpenseRepository.GetTotalUserTransaction(userId, month, year);
            responseData.TotalRecordCount = totalCount;
            if (data is not null)
            {
                responseData.TransactionsDetail = data.Take(paginationParameters.PageSize).ToList();
            }
            else
            {
                throw new Exception(ValidationResources.LogExceptionError);
            }
            return responseData;
        }

        public async Task<string> AddIncomeExpense(IncomeExpenseRequestDto incomeExpenseRequestDto, int categoryTypeId, int userId)
        {
            if (!Enum.IsDefined(typeof(CategoryTypeEnum), categoryTypeId))
            {
                throw new Exception(ResponseResources.CategoryTypeIdError);
            }
            if (!await _incomeExpenseRepository.CheckCategoryByCategoryId(incomeExpenseRequestDto.CategoryId, categoryTypeId))
            {
                throw new Exception(ResponseResources.CategoryIdError);
            }
            await _incomeExpenseRepository.AddIncomeExpense(incomeExpenseRequestDto, userId);
            return ResponseResources.AddTransactionSuccess;
        }
        public async Task<ExportTransactionDto> TransactionDataToPdf(int userId, int month, int year)
        {
            var userTransactions = await _incomeExpenseRepository.UserTransactionData(userId, month, year);
            if (userTransactions is null || userTransactions.Count is ResponseResources.Zero)
            {
                throw new Exception(ValidationResources.NoTransactionData);
            }
            string base64String = GeneratePdf.GeneratePdfDocument(month, year, userTransactions, _imagePath.Path);

            var formattedDate = new DateTime(year, month, 1).ToString(ResponseResources.Date_Format);
            var fileName = $"{ResponseResources.FileNamePrefix}{formattedDate}{ResponseResources.FileExtension}";

            return new ExportTransactionDto { ExportUrl = base64String, FileName = fileName };
        }
        public async Task<string> DeleteIncomeExpense(int userId, int loggedInUserId, int transactionId)
        {
            var incomeExpenseData = await _incomeExpenseRepository.GetTransactionById(userId, transactionId);
            if (incomeExpenseData is not null)
            {
                incomeExpenseData.DeletedBy = loggedInUserId;
                incomeExpenseData.DeletedDate = DateTime.Now;
                await _incomeExpenseRepository.DeleteIncomeExpense(incomeExpenseData);
                return ResponseResources.DeleteTransactionSuccess;
            }
            else
            {
                throw new Exception(ResponseResources.TransactionNotFound);
            }

        }

        public async Task<string> UpdateIncomeExpense(int transactionId, int loggedInUserId, int userId, IncomeExpenseRequestDto incomeExpenseRequestDto)
        {
            var incomeExpenseData = await _incomeExpenseRepository.GetIncomeExpenseById(transactionId, userId);
            if (incomeExpenseData != null)
            {
                incomeExpenseData.CategoryId = incomeExpenseRequestDto.CategoryId;
                incomeExpenseData.TransactionDate = incomeExpenseRequestDto.TransactionDate;
                incomeExpenseData.ModifiedBy = loggedInUserId;
                incomeExpenseData.ModifiedDate = DateTime.Now;
                incomeExpenseData.Description = incomeExpenseRequestDto.Description;
                incomeExpenseData.Amount = incomeExpenseRequestDto.Amount;
            }
            else
            {
                throw new Exception(ResponseResources.InvalidTransaction);
            }
            await _incomeExpenseRepository.UpdateIncomeExpense(incomeExpenseData);
            return ResponseResources.IncomeExpenseUpdateSuccess;
        }
    }
}
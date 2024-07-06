using System.ComponentModel.DataAnnotations;
using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace FinanceTracker.Controllers
{
    [Authorize(Roles = "Employee")]
    [ApiController]
    public class IncomeExpenseController : BaseController
    {
        private readonly ILogger<IncomeExpenseController> _logger;
        private readonly IIncomeExpenseService _incomeExpenseService;
        public IncomeExpenseController(ILogger<IncomeExpenseController> logger, IIncomeExpenseService incomeExpenseService, IUserDetailsService userDetailsService) : base(userDetailsService)
        {
            _logger = logger;
            _incomeExpenseService = incomeExpenseService;
        }

        [Route("users/{userId}/income-expense")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetTransactionsByCategoryType(int userId, [Required] byte categoryTypeId, [FromQuery] PaginationParameters paginationParameters, [Required][FromQuery] int month, [Required][FromQuery] int year)
        {
            try
            {
                GetUserId();
                var transactions = await _incomeExpenseService.GetTransactionsByCategoryType(userId, categoryTypeId, paginationParameters, month, year);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.UserError);
                return Error(ex.Message.ToString());
            }
        }

        [Route("users/{userId}/transaction")]
        [HttpGet]
        public async Task<IActionResult> GetUserTransaction(int userId, [FromQuery] PaginationParameters paginationParameters, [Required][FromQuery] int month, [Required][FromQuery] int year)
        {
            try
            {
                GetUserId();
                var userTransactions = await _incomeExpenseService.TransactionResponse(userId, paginationParameters, month, year);
                if (userTransactions is null)
                {
                    _logger.LogError(ValidationResources.LogNoTransactionData, userId);
                    return Error(ValidationResources.NoTransactionData);
                }
                _logger.LogInformation(ValidationResources.LogTransactionDataSuccess, userId);
                return Ok(userTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Route("users/{userId}/add-income-expense")]
        [HttpPost]
        public async Task<IActionResult> AddIncomeExpense([FromBody] IncomeExpenseRequestDto incomeExpenseRequestDto, int userId, [FromQuery][Required] int categoryTypeId)
        {
            try
            {
                var loggedInUserId = GetUserId();
                if (!ModelState.IsValid)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errorMessage.Append($"{error.ErrorMessage}; ");
                    }
                    _logger.LogWarning($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                string addTransaction = await _incomeExpenseService.AddIncomeExpense(incomeExpenseRequestDto, categoryTypeId, loggedInUserId);
                return Ok(addTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error(ex.Message.ToString());
            }
        }
        [HttpGet("users/{userId}/transaction/export-pdf")]
        public async Task<IActionResult> ExportTransactions(int userId, [Required][FromQuery] int year, [Required][FromQuery] int month)
        {
            try
            {
                GetUserId();
                var pdfData = await _incomeExpenseService.TransactionDataToPdf(userId, month, year);
                if (pdfData is null)
                {
                    _logger.LogInformation(ResponseResources.NoUserTransactionFound, userId);
                    return Error(ResponseResources.NoUserTransactionFoundError);
                }
                return Ok(pdfData, ResponseResources.ExportSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.ErrorPdfGenerate, userId);
                return Error(ex.Message.ToString());
            }
        }

        [Route("users/{userId}/income-expense/{transactionId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteIncomeExpense(int userId, int transactionId)
        {
            try
            {
                var loggedInUserId = GetUserId();
                var response = await _incomeExpenseService.DeleteIncomeExpense(userId, loggedInUserId, transactionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Error(ex.Message.ToString());
            }
        }

        [Route("users/{userId}/income-expense/{transactionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateIncomeExpense(int userId, int transactionId, [FromBody] IncomeExpenseRequestDto incomeExpenseRequestDto)
        {
            try
            {
                var loggedInUserId = GetUserId();
                if (!ModelState.IsValid)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errorMessage.Append($"{error.ErrorMessage}; ");
                    }
                    _logger.LogWarning($"{ResponseResources.InvalidModelState} {errorMessage}");
                    return Error(errorMessage.ToString());
                }
                var result = await _incomeExpenseService.UpdateIncomeExpense(transactionId, loggedInUserId, userId, incomeExpenseRequestDto);
                _logger.LogInformation(ResponseResources.IncomeExpenseUpdateSuccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error(ex.Message.ToString());
            }
        }
    }
}
using FinanceTracker.Utility.Model;

namespace FinanceTracker.Utility.Interface
{
    public interface IMailService
    {
        Task SendEmail(EmailModel emailModel);
    }
}
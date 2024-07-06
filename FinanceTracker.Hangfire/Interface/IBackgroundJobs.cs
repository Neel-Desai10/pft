using System.Linq.Expressions;

namespace FinanceTracker.Hangfire.Interface
{
    public interface IBackgroundJobs
    {
        Task SendEmail(Expression<Action> methodCall);
    }
}
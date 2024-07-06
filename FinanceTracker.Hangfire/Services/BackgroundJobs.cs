using System.Linq.Expressions;
using FinanceTracker.Hangfire.Interface;
using Hangfire;

namespace FinanceTracker.Hangfire.Services
{
    public class BackgroundJobs : IBackgroundJobs
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        public BackgroundJobs(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        public async Task SendEmail(Expression<Action> methodCall)
        {
            _backgroundJobClient.Enqueue(methodCall);
        }  
    }
}
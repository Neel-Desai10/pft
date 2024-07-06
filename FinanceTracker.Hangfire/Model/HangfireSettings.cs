namespace FinanceTracker.Hangfire.Model
{
    public class HangfireSettings
    {
        public string CronExpression { get; set; }
        public double LastRecur { get; set; }
    }
}
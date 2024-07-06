namespace FinanceTracker.Utility;

public sealed class Envelope : Envelope<string>
{
     private Envelope(object data)
            : base(null, data, null)
        {
        }
        private Envelope(string successMessage)
            : base(null, null, successMessage)
        {
        }

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, null,null);
        }
        public static Envelope<T> Ok<T>(T result,string successMessage)
        {
            return new Envelope<T>(result, null,successMessage);
        }
        public static Envelope Ok(object errorMessage)
        {
            return new Envelope(errorMessage);
        }
        public static Envelope Ok(string successMessage)
        {
            return new Envelope(successMessage);
        }
        public static Envelope ErrorMessage(object errorMessage)
        {
            return new Envelope(errorMessage);
        }
}

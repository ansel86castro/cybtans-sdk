namespace Cybtans.Services
{
    public class ErrorInfo
    {
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public int? ErrorCode { get; set; }

        public ErrorInfo() { }

        public ErrorInfo(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}

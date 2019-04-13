namespace Core.Web.Filters
{
    public class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
        }

        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}

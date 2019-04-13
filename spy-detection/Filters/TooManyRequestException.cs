using System;

namespace Core.Web.Filters
{
    public class TooManyRequestException : Exception
    {
        public int RetryAfter { get; }
        public TooManyRequestException(int retryAfter)
        {
            RetryAfter = retryAfter;
        }
    }
}

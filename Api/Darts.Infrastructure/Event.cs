using System;

namespace Darts.Infrastructure
{
    public abstract class Event
    {
    }



    public class DomainException : Exception
    {
        public DomainException(int statusCode, string failureReason)
        {
            StatusCode = statusCode;
            FailureReason = failureReason;
        }

        public int StatusCode { get; }
        public string FailureReason { get; }
    }
}

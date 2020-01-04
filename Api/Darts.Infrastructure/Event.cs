using System;

namespace Darts.Infrastructure
{
    public abstract class Event
    {
    }



    public class DomainException : Exception
    {
        public DomainException(string failureReason)
        {
            FailureReason = failureReason;
        }

        public string FailureReason { get; }
    }
}

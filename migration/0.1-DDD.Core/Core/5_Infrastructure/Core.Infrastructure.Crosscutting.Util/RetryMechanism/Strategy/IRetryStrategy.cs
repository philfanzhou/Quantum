using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public interface IRetryStrategy<T> : ILimitationStrategy<T>
    {
        TimeSpan? GetRetryInterval();

        void StartRetry();

        void EndRetry();
    }
}

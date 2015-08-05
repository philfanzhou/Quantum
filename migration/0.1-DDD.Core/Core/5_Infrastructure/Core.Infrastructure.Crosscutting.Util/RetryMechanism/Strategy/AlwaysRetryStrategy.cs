using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public class AlwaysRetryStrategy<T> : RetryStrategyBase<T>
        where T : class
    {
        public AlwaysRetryStrategy(TimeSpan retryInterval)
            : base(retryInterval)
        {
        }

        public override bool ExceedLimit()
        {
            return false;
        }
    }
}

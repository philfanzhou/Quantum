using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public abstract class RetryStrategyBase<T> : LimitationStrategyBase<T>, IRetryStrategy<T>
        where T : class
    {
        private TimeSpan? retryInterval;

        public RetryStrategyBase()
        {
        }

        public RetryStrategyBase(TimeSpan retryInterval)
        {
            this.retryInterval = retryInterval;
        }

        protected TimeSpan? RetryInterval
        {
            get { return this.retryInterval; }
        }

        public TimeSpan? GetRetryInterval()
        {
            return this.retryInterval;
        }

        public virtual void StartRetry()
        {
        }

        public virtual void EndRetry()
        {
        }
    }
}

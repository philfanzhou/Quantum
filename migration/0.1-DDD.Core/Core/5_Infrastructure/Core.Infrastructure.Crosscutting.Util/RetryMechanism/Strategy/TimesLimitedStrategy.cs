using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public class TimesLimitedStrategy<T> : RetryStrategyBase<T>
        where T : class
    {
        private int timesLimited = 0;

        public TimesLimitedStrategy(int timesLimited)
        {
            this.timesLimited = timesLimited;
        }

        public TimesLimitedStrategy(int timesLimited, TimeSpan retryInterval)
            : base(retryInterval)
        {
            this.timesLimited = timesLimited;
        }

        public override bool ExceedLimit()
        {
            return GetCount() > this.timesLimited;
        }
    }
}

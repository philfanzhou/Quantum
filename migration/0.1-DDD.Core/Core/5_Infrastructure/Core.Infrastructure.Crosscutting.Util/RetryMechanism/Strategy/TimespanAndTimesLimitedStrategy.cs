using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public class TimespanAndTimesLimitedStrategy<T> : TimespanLimitedStrategy<T>
        where T : class
    {
        private int timesLimited;
        
        public TimespanAndTimesLimitedStrategy(TimeSpan timespanLimited, int timesLimited)
            : base(timespanLimited)
        {
            this.timesLimited = timesLimited;
        }

        public override void Add(T instance)
        {
            if (this.ExceedLimit() || GetCount() == 0)
            {
                ResetCount();
                StartRetry();
            }

            base.Add(instance);
        }

        public override bool ExceedLimit()
        {
            return GetCount() > this.timesLimited;
        }
    }
}

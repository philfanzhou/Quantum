using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public class TimespanLimitedStrategy<T> : RetryStrategyBase<T>
        where T : class
    {
        private TimeSpan timeSpanLimited = TimeSpan.FromMinutes(2);        

        public TimespanLimitedStrategy(TimeSpan timespanLimited)
        {
            this.timeSpanLimited = timespanLimited;
        }

        public TimespanLimitedStrategy(TimeSpan timespanLimited, TimeSpan retryInterval)
            : base(retryInterval)
        {
            this.timeSpanLimited = timespanLimited;
        }

        public DateTime? StartTiming { get; private set; }

        public override bool ExceedLimit()
        {
            if (this.StartTiming != null)
            {
                return this.StartTiming.Value.Add(this.timeSpanLimited) < DateTime.Now;
            }

            return false;
        }

        public override void StartRetry()
        {
            this.StartTiming = DateTime.Now;
        }        
    }
}

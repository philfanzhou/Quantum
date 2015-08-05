using System;
using System.Threading;
using Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism
{
    public class FuncRetry<T, TResult> : RetryBase<T>
    {
        public FuncRetry(IRetryStrategy<Exception> retryStrategy)
            : base(retryStrategy)
        {
        }

        public void ExecuteAsync(Func<T, TResult> retryAction, T instance)
        {
            WaitCallback fire = (subscriber) =>
            {
                var result = Execute(retryAction, instance);
            };
            ThreadPool.QueueUserWorkItem(fire, retryAction);
        }
    }
}
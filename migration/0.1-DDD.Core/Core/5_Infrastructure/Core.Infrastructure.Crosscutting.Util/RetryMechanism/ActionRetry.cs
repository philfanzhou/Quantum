using System;
using System.Threading;
using Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism
{
    public class ActionRetry<T> : RetryBase<T>
    {
        public ActionRetry(IRetryStrategy<Exception> retryStrategy)
            : base(retryStrategy)
        {
        }

        public Action<T> RetryAction
        {
            get;
            protected set;
        }

        public virtual void ExecuteAsync(Action<T> retryAction, T instance)
        {
            this.RetryAction = retryAction;
            WaitCallback fire = (subscriber) =>
            {
                Execute(retryAction, instance);
            };
            ThreadPool.QueueUserWorkItem(fire, retryAction);
        }
    }
}

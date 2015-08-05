using System;
using System.Threading;
using Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism
{
    public class RetryBase<T>
    {
        private IRetryStrategy<Exception> retryStrategy = null;

        public RetryBase(IRetryStrategy<Exception> strategy)
        {
            this.retryStrategy = strategy;
        }

        public delegate void OnCloseHandler();

        public delegate void OnErrorHandler(Exception ex);

        public delegate void OnRetryExhaustedHandler(Exception ex, T instance);

        public delegate void OnSucceedHandler(T instance);

        public event OnSucceedHandler OnSucceed;

        public event OnCloseHandler OnClose;

        public event OnErrorHandler OnError;

        public event OnRetryExhaustedHandler OnRetryExhausted;

        public TResult Execute<TResult>(Func<T, TResult> retryAction, T instance)
        {
            Exception lastException = null;

            try
            {
                while (!this.retryStrategy.ExceedLimit())
                {
                    try
                    {
                        lastException = null;
                        var result = retryAction(instance);
                        this.RaiseOnSucceed(instance);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        this.RaiseOnErrorEvent(ex);
                        this.retryStrategy.Add(ex);
                    }

                    var retryInterval = this.retryStrategy.GetRetryInterval();
                    if (retryInterval.HasValue)
                    {
                        Thread.Sleep(retryInterval.Value);
                    }
                }

                this.RaiseOnRetryExhaustedEvent(lastException, instance);
            }
            finally
            {
                this.RaiseOnCloseEvent();
            }

            return default(TResult);
        }

        public void Execute(Action<T> retryAction, T instance)
        {
            Exception lastException = null;

            try
            {
                this.retryStrategy.ResetCount();
                this.retryStrategy.StartRetry();
                bool needSleep = false;
                while (!this.retryStrategy.ExceedLimit())
                {
                    if (needSleep)
                    {
                        var retryInterval = this.retryStrategy.GetRetryInterval();
                        if (retryInterval.HasValue)
                        {
                            Thread.Sleep(retryInterval.Value);
                        }
                    }

                    needSleep = true;
                    try
                    {
                        lastException = null;
                        retryAction(instance);
                        this.RaiseOnSucceed(instance);
                        return;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        this.RaiseOnErrorEvent(ex);
                        this.retryStrategy.Add(ex);
                    }
                }

                this.RaiseOnRetryExhaustedEvent(lastException, instance);
            }
            finally
            {
                this.retryStrategy.EndRetry();
                this.RaiseOnCloseEvent();
            }
        }

        protected virtual void RaiseOnRetryExhaustedEvent(Exception ex, T instance)
        {
            if (this.OnRetryExhausted != null)
            {
                this.OnRetryExhausted(ex, instance);
            }
        }

        protected virtual void RaiseOnErrorEvent(Exception ex)
        {
            if (this.OnError != null)
            {
                this.OnError(ex);
            }
        }

        protected virtual void RaiseOnCloseEvent()
        {
            if (this.OnClose != null)
            {
                this.OnClose();
            }
        }

        protected virtual void RaiseOnSucceed(T instance)
        {
            if (this.OnSucceed != null)
            {
                this.OnSucceed(instance);
            }
        }
    }
}

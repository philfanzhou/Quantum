using System;

namespace Core.DistributedServices.WCF
{
    public interface IServiceClient<TChannel>
     where TChannel : class
    {
        TResult Invoke<TResult>(Func<TChannel, TResult> function);

        void Invoke(Action<TChannel> action);
    }
}

using System;

namespace Core.DistributedServices.WCF
{
    public interface IDuplexServiceClient<T>
     where T : class
    {
        event EventHandler OnProxyFaulted;

        T Invoker { get; }        
    }
}

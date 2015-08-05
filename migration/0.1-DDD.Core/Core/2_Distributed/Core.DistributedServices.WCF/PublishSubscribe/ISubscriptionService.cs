using System;
using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    [ServiceContract]
    public interface ISubscriptionService<TEvent, TSubscriber, TSubscribeResult> : IWCFService
        where TEvent : class
        where TSubscriber : Subscriber
        where TSubscribeResult : SubscribeResult
    {
        [OperationContract]
        TSubscribeResult Subscribe(string eventOperation, TSubscriber subscriber);

        [OperationContract]
        void Unsubscribe(string eventOperation, Guid subscribeId);
    }
}

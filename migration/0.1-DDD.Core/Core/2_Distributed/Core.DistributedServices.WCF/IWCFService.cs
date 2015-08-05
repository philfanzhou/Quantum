using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        bool IsAlive();
    }
}

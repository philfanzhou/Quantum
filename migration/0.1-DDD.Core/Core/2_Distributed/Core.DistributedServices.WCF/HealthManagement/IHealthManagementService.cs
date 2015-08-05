using System.Collections.Generic;
using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    [ServiceContract]
    public interface IHealthManagementService : IWCFService
    {
        [OperationContract]
        IList<WCFServiceHealthInfo> GetAllHostedWCFServiceStatus();
    }
}

using System.Collections.Generic;
using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HealthManagementService : WCFServiceBase, IHealthManagementService
    {
        internal delegate IList<WCFServiceHealthInfo> GetAllHostedWCFServiceStatusHandler();

        internal GetAllHostedWCFServiceStatusHandler GetAllWCFServiceStatus { get; set; }

        public IList<WCFServiceHealthInfo> GetAllHostedWCFServiceStatus()
        {
            return this.GetAllWCFServiceStatus();
        }
    }
}

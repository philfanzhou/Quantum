namespace Core.DistributedServices.WCF
{
    public abstract class WCFServiceBase : IWCFService
    {
        public WCFServiceBase()
        {
        }

        public virtual bool IsAlive()
        {
            return true;
        }
    }
}

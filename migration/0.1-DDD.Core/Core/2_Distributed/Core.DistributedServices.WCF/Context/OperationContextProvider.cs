using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    public class OperationContextProvider
    {
        private static IOperationContext operationContext = null;

        public static IOperationContext Current
        {
            get
            {
                return OperationContextProvider.operationContext == null ?
                    OperationContext.Current == null ? null : new OperationContextWrapper(OperationContext.Current)
                    : operationContext;
            }
        }
#if DEBUG
        public static void SetOperationContext(IOperationContext operationContext)
        {
            OperationContextProvider.operationContext = operationContext;
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Core.DistributedServices.WCF
{
    public interface IOperationContext
    {
        event EventHandler OperationCompleted;

        IContextChannel Channel { get; }

        IExtensionCollection<OperationContext> Extensions { get; }

        bool HasSupportingTokens { get; }

        ServiceHostBase Host { get; }

        MessageHeaders IncomingMessageHeaders { get; }

        MessageProperties IncomingMessageProperties { get; }

        MessageVersion IncomingMessageVersion { get; }

        InstanceContext InstanceContext { get; }

        bool IsUserContext { get; }

        MessageHeaders OutgoingMessageHeaders { get; }

        MessageProperties OutgoingMessageProperties { get; }

        RequestContext RequestContext { get; set; }

        string SessionId { get; }

        ICollection<SupportingTokenSpecification> SupportingTokens { get; }

        T GetCallbackChannel<T>();

        void SetTransactionComplete();
    }
}

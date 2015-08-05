using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Core.DistributedServices.WCF
{
    public class OperationContextWrapper : IOperationContext
    {
        private OperationContext context;

        public OperationContextWrapper(OperationContext context)
        {
            this.context = context;
        }

        public event EventHandler OperationCompleted;

        public IContextChannel Channel
        {
            get { return this.context.Channel; }
        }

        public IExtensionCollection<OperationContext> Extensions
        {
            get { return this.context.Extensions; }
        }

        public bool HasSupportingTokens
        {
            get { return this.context.HasSupportingTokens; }
        }

        public ServiceHostBase Host
        {
            get { return this.context.Host; }
        }

        public MessageHeaders IncomingMessageHeaders
        {
            get { return this.context.IncomingMessageHeaders; }
        }

        public MessageProperties IncomingMessageProperties
        {
            get { return this.context.IncomingMessageProperties; }
        }

        public MessageVersion IncomingMessageVersion
        {
            get { return this.context.IncomingMessageVersion; }
        }

        public InstanceContext InstanceContext
        {
            get { return this.context.InstanceContext; }
        }

        public bool IsUserContext
        {
            get { return this.context.IsUserContext; }
        }

        public MessageHeaders OutgoingMessageHeaders
        {
            get { return this.context.OutgoingMessageHeaders; }
        }

        public MessageProperties OutgoingMessageProperties
        {
            get { return this.context.OutgoingMessageProperties; }
        }

        public RequestContext RequestContext
        {
            get
            {
                return this.context.RequestContext;
            }

            set
            {
                this.context.RequestContext = value;
            }
        }

        public string SessionId
        {
            get { return this.context.SessionId; }
        }

        public ICollection<SupportingTokenSpecification> SupportingTokens
        {
            get { return this.context.SupportingTokens; }
        }

        public T GetCallbackChannel<T>()
        {
            return this.context.GetCallbackChannel<T>();
        }

        public void SetTransactionComplete()
        {
            this.context.SetTransactionComplete();
        }
    }
}

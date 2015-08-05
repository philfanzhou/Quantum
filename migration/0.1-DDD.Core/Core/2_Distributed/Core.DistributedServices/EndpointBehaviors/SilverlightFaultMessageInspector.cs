//namespace Core.DistributedServices
//{
//    using System.ServiceModel;
//    using System.ServiceModel.Channels;
//    using System.ServiceModel.Dispatcher;

//    /// <summary>
//    /// By default, WCF services return fault messages with an HTTP 500 response code.
//    /// Due to limitations in the browser networking stack, the bodies of these messages are inaccessible within Silverlight, 
//    /// and consequently the fault messages cannot be read by the client.
//    /// To send faults that will be accessible to a Silverlight client, a WCF service must modify the way it sends its fault messages.
//    /// The key change needed is for WCF to return fault messages with an HTTP 200 response code instead of the HTTP 500 response code.
//    /// This change enables Silverlight to read the body of the message and also enables WCF clients of the same service to continue
//    /// working using their normal fault-handling procedures.
//    /// </summary>
//    /// <remarks>
//    /// http://msdn.microsoft.com/en-us/library/ee844556%28VS.96%29.aspx
//    /// </remarks>
//    public class SilverlightFaultMessageInspector : IDispatchMessageInspector
//    {
//        public void BeforeSendReply(ref Message reply, object correlationState)
//        {
//            if (reply.IsFault)
//            {
//                var property = new HttpResponseMessageProperty();

//                // Here the response code is changed to 200.
//                property.StatusCode = System.Net.HttpStatusCode.OK;

//                reply.Properties[HttpResponseMessageProperty.Name] = property;
//            }
//        }

//        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
//        {
//            // Do nothing to the incoming message.
//            return null;
//        }
//    }

//}

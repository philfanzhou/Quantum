using System;
using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    public class ServiceInvoker
    {
        public static void Invoke<TChannel>(Action<TChannel> action, TChannel proxy)
        {
            ICommunicationObject channel = proxy as ICommunicationObject;
            if (null == channel)
            {
                throw new ArgumentException("The proxy is not a valid channel implementing the ICommunicationObject interface", "proxy");
            }

            try
            {
                channel.Open();
                action(proxy);
            }
            catch (TimeoutException)
            {
                channel.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                channel.Abort();
                throw;
            }
            finally
            {
                channel.Close();
            }
        }

        public static TResult Invoke<TChannel, TResult>(Func<TChannel, TResult> function, TChannel proxy)
        {
            ICommunicationObject channel = proxy as ICommunicationObject;
            if (null == channel)
            {
                throw new ArgumentException("The proxy is not a valid channel implementing the ICommunicationObject interface", "proxy");
            }

            try
            {
                if (channel.State != CommunicationState.Opened)
                {
                    channel.Open();
                }

                return function(proxy);
            }
            catch (TimeoutException)
            {
                channel.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                channel.Abort();
                throw;
            }
            finally
            {
                channel.Close();
            }
        }
    }
}

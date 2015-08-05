using System;

namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Entity
{
    [Serializable]
    public class AutoRetryFailedMessage<T>
    {
        public Action<T> Action { get; set; }

        public T Instance { get; set; }

        public long MessageId { get; set; }
    }
}

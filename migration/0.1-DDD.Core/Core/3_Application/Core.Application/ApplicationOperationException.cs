namespace Core.Application
{
    using System;
    using System.Runtime.Serialization;

    public class ApplicationOperationException : Exception
    {
        public ApplicationOperationException() : base()
        {
        }

        public ApplicationOperationException(string message) : base(message)
        {
        }

        public ApplicationOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ApplicationOperationException(SerializationInfo info, StreamingContext contex)
            : base(info, contex)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class NoBatteryException : Exception
    {
        public NoBatteryException()
        {
        }

        public NoBatteryException(string message) : base(message)
        {
        }

        public NoBatteryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoBatteryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
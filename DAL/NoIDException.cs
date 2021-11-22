using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class NoIDException : Exception
    {
        public NoIDException()
        {
        }

        public NoIDException(string message) : base(message)
        {
        }

        public NoIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
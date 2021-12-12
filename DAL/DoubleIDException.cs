using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class DoubleIDException : Exception
    {
        public DoubleIDException()
        {
        }

        public DoubleIDException(string message) : base(message)
        {
        }

        public DoubleIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoubleIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
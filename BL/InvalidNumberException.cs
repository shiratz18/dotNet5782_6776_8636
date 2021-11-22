using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class InvalidNumberException : Exception
    {
        public InvalidNumberException()
        {
        }

        public InvalidNumberException(string message) : base(message)
        {
        }

        public InvalidNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
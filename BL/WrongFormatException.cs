using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class WrongFormatException : Exception
    {
        public WrongFormatException()
        {
        }

        public WrongFormatException(string message) : base(message)
        {
        }

        public WrongFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
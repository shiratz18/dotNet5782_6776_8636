using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class CannotDeleteException : Exception
    {
        public CannotDeleteException()
        {
        }

        public CannotDeleteException(string message) : base(message)
        {
        }

        public CannotDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
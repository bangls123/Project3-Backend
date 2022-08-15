using System;
using System.Runtime.Serialization;

namespace Vnext.Intern.WinServiceBase.Utils
{
    [Serializable]
    public class TypeInstatiationException : Exception
    {
        public TypeInstatiationException()
        {
        }

        public TypeInstatiationException(string message) : base(message)
        {
        }

        public TypeInstatiationException(string message, Exception innerException) : base(message,
            innerException)
        {
        }

        protected TypeInstatiationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

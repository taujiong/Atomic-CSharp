using System;
using System.Runtime.Serialization;

namespace Atomic.ExceptionHandling
{
    /// <summary>
    /// This exception type is directly shown to the user.
    /// </summary>
    [Serializable]
    public class BusinessException : AtomicException, IHasErrorCode, IHasErrorDetails
    {
        public BusinessException(
            string code = null,
            string message = null,
            string details = null,
            Exception innerException = null
        ) : base(message, innerException)
        {
            Code = code;
            Details = details;
        }

        public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public string Code { get; set; }

        public string Details { get; set; }
    }
}
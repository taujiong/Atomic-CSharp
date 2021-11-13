using System;

namespace Atomic.ExceptionHandling
{
    /// <summary>
    /// Used to store information about an error.
    /// </summary>
    [Serializable]
    public class UserFriendlyErrorInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserFriendlyErrorInfo"/>.
        /// </summary>
        public UserFriendlyErrorInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="UserFriendlyErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="details">Error details</param>
        /// <param name="message">Error message</param>
        public UserFriendlyErrorInfo(string message, string details = null, string code = null)
        {
            Message = message;
            Details = details;
            Code = code;
        }

        /// <summary>
        /// Error code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// </summary>
        public ValidationErrorInfo[] ValidationErrors { get; set; }
    }
}
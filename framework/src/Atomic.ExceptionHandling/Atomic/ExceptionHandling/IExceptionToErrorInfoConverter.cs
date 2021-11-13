using System;

namespace Atomic.ExceptionHandling
{
    public interface IExceptionToErrorInfoConverter
    {
        UserFriendlyErrorInfo Convert(Exception exception, bool includeSensitiveDetails);
    }
}
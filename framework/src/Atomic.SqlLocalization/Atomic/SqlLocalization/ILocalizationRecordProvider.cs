using System.Collections.Generic;

namespace Atomic.SqlLocalization
{
    public interface ILocalizationRecordProvider
    {
        Dictionary<string, string> GetAllLocalizations(string resourceKey);
    }
}
using System.Collections.Generic;

namespace Atomic.Localization.Abstraction
{
    public interface ILocalizationRecordProvider
    {
        Dictionary<string, string> GetAllLocalizations(string resourceKey);
    }
}
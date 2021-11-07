using System;
using System.Collections.Concurrent;
using Atomic.Utils;
using Microsoft.Extensions.Localization;

namespace Atomic.SqlLocalization
{
    public class AtomicStringLocalizerFactory : IStringLocalizerFactory
    {
        private static readonly ConcurrentDictionary<string, IStringLocalizer> LocalizerCache = new();
        private readonly ILocalizationRecordProvider _localizationRecordProvider;

        public AtomicStringLocalizerFactory(
            ILocalizationRecordProvider localizationRecordProvider
        )
        {
            _localizationRecordProvider = localizationRecordProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var resourceKey = resourceSource.FullName;
            Check.NotNull(resourceKey, nameof(resourceSource.FullName));
            return CreateFromResourceKey(resourceKey);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var resourceKey = baseName + location;
            return CreateFromResourceKey(resourceKey);
        }

        private IStringLocalizer CreateFromResourceKey(string resourceKey)
        {
            Check.NotNull(resourceKey, nameof(resourceKey));
            return LocalizerCache.GetOrAdd(
                resourceKey,
                key => new AtomicStringLocalizer(_localizationRecordProvider.GetAllLocalizations(key), resourceKey)
            );
        }
    }
}
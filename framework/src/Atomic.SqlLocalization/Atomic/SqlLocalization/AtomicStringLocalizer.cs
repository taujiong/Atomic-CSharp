using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Atomic.Utils;
using Microsoft.Extensions.Localization;

namespace Atomic.SqlLocalization
{
    public class AtomicStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _localizationRecords;

        private readonly ConcurrentDictionary<string, object?> _missingResourceKey;

        private readonly string _resourceKey;

        public AtomicStringLocalizer(Dictionary<string, string> localizationRecords, string resourceKey)
        {
            _localizationRecords = localizationRecords;
            _resourceKey = resourceKey;
            _missingResourceKey = new ConcurrentDictionary<string, object?>();
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            GetAllStrings(includeParentCultures, CultureInfo.CurrentCulture);

        public LocalizedString this[string name]
        {
            get
            {
                Check.NotNull(name, nameof(name));
                var value = GetStringSafely(name, null);
                return new LocalizedString(name, value ?? name, value == null, _resourceKey);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                Check.NotNull(name, nameof(name));
                var format = GetStringSafely(name, null);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value ?? name, value == null, _resourceKey);
            }
        }

        private string? GetStringSafely(string key, CultureInfo? culture)
        {
            Check.NotNull(key, nameof(key));
            culture ??= CultureInfo.CurrentCulture;

            var searchKey = $"{key}.{culture.Name}";
            if (_missingResourceKey.ContainsKey(searchKey))
            {
                return null;
            }

            var hasKey = _localizationRecords.TryGetValue(searchKey, out var value);
            if (!hasKey)
            {
                _missingResourceKey.TryAdd(searchKey, null);
            }

            return value;
        }

        private IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            Check.NotNull(culture, nameof(culture));

            var cultureNames = includeParentCultures
                ? GetAllParentCultureNames(culture)
                : new HashSet<string> { culture.Name };

            foreach (var (key, value) in _localizationRecords)
            {
                var separatorIndex = key.LastIndexOf(".", StringComparison.InvariantCulture);
                var cultureName = key[(separatorIndex + 1)..];

                if (cultureNames.Contains(cultureName))
                {
                    var localizationKey = key[..separatorIndex];
                    yield return new LocalizedString(localizationKey, value, true, _resourceKey);
                }
            }
        }

        private static HashSet<string> GetAllParentCultureNames(CultureInfo culture)
        {
            var cultureNames = new HashSet<string>();
            while (!culture.Equals(culture.Parent))
            {
                cultureNames.Add(culture.Name);
                culture = culture.Parent;
            }

            return cultureNames;
        }
    }
}
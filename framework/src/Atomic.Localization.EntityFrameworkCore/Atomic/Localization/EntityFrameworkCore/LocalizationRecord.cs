namespace Atomic.Localization.EntityFrameworkCore
{
    public class LocalizationRecord
    {
        public LocalizationRecord(
            string resourceKey,
            string culture,
            string key,
            string value,
            bool needUpdate = true
        )
        {
            ResourceKey = resourceKey;
            Culture = culture;
            Key = key;
            Value = value;
            NeedUpdate = needUpdate;
        }

        public long Id { get; set; }

        public string ResourceKey { get; set; }

        public string Culture { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool NeedUpdate { get; set; }
    }
}
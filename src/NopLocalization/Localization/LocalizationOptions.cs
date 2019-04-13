using Microsoft.AspNetCore.Builder;
using System;

namespace NopLocalization
{
    public class LocalizationOptions
    {
        /// <summary>
        /// Gets or sets default language code
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets other languages code
        /// </summary>
        public string[] OtherLanguages { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocalizedProperties (default: false)
        /// </summary>
        public bool LoadAllLocalizedProperties { get; set; } = false;

        /// <summary>
        /// Gets or sets cache time in minutes (default: 60)
        /// </summary>
        public int CacheTimeInMinutes { get; set; } = 60;

        /// <summary>
        /// Gets or sets cache mode (default: MemoryCache)
        /// </summary>
        public CacheMode CacheMode { get; set; } = CacheMode.MemoryCache;

        /// <summary>
        /// Gets or sets Redis connection string like (default: "localhost:6379" or "127.0.0.1:6379,ssl=False")
        /// </summary>
        public string RedisCachingConnectionString { get; set; } = "localhost:6379";

        /// <summary>
        /// Get or set culture segment index in url (default: 0) for example routing => "{culture}/{controller}/{action}
        /// </summary>
        public int CultureSegmentIndex { get; set; } = 0;

        public Action<RequestLocalizationOptions> ConfigureRequestLocalizationOptions { get; set; } = null;
    }

    public enum CacheMode
    {
        MemoryCache,
        RedisCacheWithProtoBuf
    }
}

namespace NopLocalization
{
    public  class LocalizationCacheKeys
    {
        /// <summary>
        /// Key for caching a property
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// {1} : entity id
        /// {2} : property name
        /// {3} : language id
        /// </remarks>
        public const string LocalizedPropertyCacheKey = "LocalizedProperty-{0}-{1}-{2}-{3}";
        public const string LocalizedPropertyAllCacheKey = "LocalizedProperty-All";
        public const string LocalizedPropertyCacheRegion = "LocalizedProperty";

        public const string LanguageAllCacheKey = "Language-All";
        public const string LanguageCacheRegion = "Language";
    }
}

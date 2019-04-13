using System;

namespace NopLocalization
{
    /// <summary>
    /// Language entity for caching
    /// </summary>
    [Serializable]
    public class LanguageCached
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the two letter iso language name 
        /// </summary>
        public virtual string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}

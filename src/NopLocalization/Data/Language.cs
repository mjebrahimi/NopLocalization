using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NopLocalization
{
    /// <summary>
    /// Language entity
    /// </summary>
    public partial class Language
    {
        //public Language(string name, string twoLetterISOLanguageName)
        //{
        //    Name = name;
        //    TwoLetterISOLanguageName = twoLetterISOLanguageName;
        //}

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the two letter iso language name  like "fa", "en", ...
        /// </summary>
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public virtual string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        public virtual ICollection<LocalizedProperty> LocalizedProperties { get; set; }

        public override string ToString()
        {
            return $"{Name} ({LanguageCode})";
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NopLocalization
{
    /// <summary>
    /// Represents a localized property
    /// </summary>
    public partial class LocalizedProperty
    {
        //public LocalizedProperty(int entityId, string entityName, string propertyName, string localeValue, int languageId)
        //{
        //    EntityId = entityId;
        //    EntityName = entityName;
        //    PropertyName = propertyName;
        //    LocaleValue = localeValue;
        //    LanguageId = languageId;
        //}

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        [Required]
        [StringLength(100)]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the property name
        /// </summary>
        [Required]
        [StringLength(100)]
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the locale value
        /// </summary>
        [Required]
        public virtual string LocaleValue { get; set; }

        /// <summary>
        /// Gets the language
        /// </summary>
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
    }
}

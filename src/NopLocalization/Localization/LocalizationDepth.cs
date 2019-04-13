namespace NopLocalization
{
    public enum LocalizationDepth
    {
        /// <summary>
        /// Localizes only root objects (Default)
        /// </summary>
        Shallow,

        /// <summary>
        /// Localizes only root objects and immediate children (properties and collections)
        /// </summary>
        OneLevel,

        /// <summary>
        /// Localizes only root objects and all children recursively
        /// </summary>
        Deep
    }
}

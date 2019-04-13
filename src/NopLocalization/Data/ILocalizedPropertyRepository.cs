using NopLocalization.Internal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public interface ILocalizedPropertyRepository: IRepository<LocalizedProperty>
    {
        List<LocalizedPropertyCached> GetAllCached();
        Task<List<LocalizedPropertyCached>> GetAllCachedAsync(CancellationToken cancellationToken = default);
        string GetLocalizedValue(string entityName, string propertyName, int entityId, int languageId, bool loadAllLocalizedProperties);
        Task<string> GetLocalizedValueAsync(string entityName, string propertyName, int entityId, int languageId, bool loadAllLocalizedProperties, CancellationToken cancellationToken = default);
        void SaveLocalizedValue<T>(string entityName, int entityId, string propertyName, T localeValue, int languageId);
        Task SaveLocalizedValueAsync<T>(string entityName, int entityId, string propertyName, T localeValue, int languageId, CancellationToken cancellationToken = default);
    }
}
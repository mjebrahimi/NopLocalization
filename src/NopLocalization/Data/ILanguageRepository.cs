using NopLocalization.Internal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public interface ILanguageRepository : IRepository<Language>
    {
        List<LanguageCached> GetAllCached();
        Task<List<LanguageCached>> GetAllCachedAsync(CancellationToken cancellationToken = default);
        LanguageCached GetByIdCached(int id);
        Task<LanguageCached> GetByIdCachedAsync(int id, CancellationToken cancellationToken = default);
    }
}
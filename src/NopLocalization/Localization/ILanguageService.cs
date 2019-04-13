using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public interface ILanguageService
    {
        Language GetCurrentLanguage();
        Task<Language> GetCurrentLanguageAsync(CancellationToken cancellationToken = default);
        LanguageCached GetCurrentLanguageCached();
        Task<LanguageCached> GetCurrentLanguageCachedAsync(CancellationToken cancellationToken = default);
        string GetCurrentLanguageCode(bool throwExceptionIfNotExists = false);
        Task<string> GetCurrentLanguageCodeAsync(CancellationToken cancellationToken = default, bool throwExceptionIfNotExists = false);
        int GetCurrentLanguageId();
        Task<int> GetCurrentLanguageIdAsync(CancellationToken cancellationToken = default);
    }
}
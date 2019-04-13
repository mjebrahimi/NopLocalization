using NopLocalization.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class LanguageService : ILanguageService
    {
        #region Fields
        //private readonly IEnumerable<ILanguageResolver> _languageResolvers;
        private readonly ILanguageResolver _languageResolver;
        private readonly ILanguageRepository _languageRepository;
        #endregion

        #region Constructor
        public LanguageService(/*IEnumerable<ILanguageResolver> languageResolvers,*/
            ILanguageResolver languageResolver,
            ILanguageRepository languageRepository)
        {
            //_languageResolvers = languageResolvers;
            _languageResolver = languageResolver;
            _languageRepository = languageRepository;
        }
        #endregion

        #region Async Methods
        public virtual async Task<string> GetCurrentLanguageCodeAsync(CancellationToken cancellationToken = default, bool throwExceptionIfNotExists = true)
        {
            //foreach (var languageResolver in _languageResolvers)
            //{
            //    var languageCode = languageResolver.GetCurrentLanguageCode();
            //    if (languageCode.HasValue())
            //    {
            //        if (validateLanguageExists)
            //        {
            //            var exist = (await _languageRepository.GetAllCachedAsync(cancellationToken))
            //                .Any(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
            //            if (!exist) throw new Exception($"Current lagnuage '{languageCode}' dose not exists in languages table.");
            //        }
            //        return languageCode;
            //    }
            //}
            //throw new InvalidOperationException("Current language not found!");

            //TODO: return default language [OR] throw exception when language not found ? 
            var languageCode = _languageResolver.GetCurrentLanguageCode();
            if (throwExceptionIfNotExists)
            {
                var exists = (await _languageRepository.GetAllCachedAsync(cancellationToken))
                    .Any(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

                if (!exists) throw new Exception($"Current lagnuage '{languageCode}' dose not exists in languages table.");
            }
            return languageCode;
        }

        public virtual async Task<int> GetCurrentLanguageIdAsync(CancellationToken cancellationToken = default)
        {
            var language = await GetCurrentLanguageCachedAsync(cancellationToken);

            //TODO: return 0  [OR] default language id [OR] throw exception if not found
            //return language?.Id ?? throw null;
            return language.Id;
        }

        public virtual async Task<LanguageCached> GetCurrentLanguageCachedAsync(CancellationToken cancellationToken = default)
        {
            var languageCode = await GetCurrentLanguageCodeAsync(cancellationToken: cancellationToken);

            var language = (await _languageRepository.GetAllCachedAsync(cancellationToken))
                .Where(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return language;// ?? throw null;
        }

        public virtual async Task<Language> GetCurrentLanguageAsync(CancellationToken cancellationToken = default)
        {
            var languageCode = await GetCurrentLanguageCodeAsync(cancellationToken);

            var language = await _languageRepository.Table
                .Where(p => p.LanguageCode == languageCode) //TODO: check StringComparison.OrdinalIgnoreCase
                .FirstOrDefaultAsync(cancellationToken);

            return language;// ?? throw null;
        }
        #endregion

        #region Sync Methods
        public virtual string GetCurrentLanguageCode(bool throwExceptionIfNotExists = true)
        {
            var languageCode = _languageResolver.GetCurrentLanguageCode();
            if (throwExceptionIfNotExists)
            {
                var exists = _languageRepository.GetAllCached()
                    .Any(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

                if (!exists) throw new Exception($"Current lagnuage '{languageCode}' dose not exists in languages table.");
            }
            return languageCode;
        }

        public virtual int GetCurrentLanguageId()
        {
            var language = GetCurrentLanguageCached();
            return language.Id;
        }

        public virtual LanguageCached GetCurrentLanguageCached()
        {
            var languageCode = GetCurrentLanguageCode();

            var language = _languageRepository.GetAllCached()
                .Where(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return language;
        }

        public virtual Language GetCurrentLanguage()
        {
            var languageCode = GetCurrentLanguageCode();

            var language = _languageRepository.Table
                .Where(p => p.LanguageCode == languageCode) //TODO: check StringComparison.OrdinalIgnoreCase
                .FirstOrDefault();

            return language;
        }
        #endregion
    }
}

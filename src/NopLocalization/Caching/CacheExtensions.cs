using CacheManager.Core;
using System;
using System.Threading.Tasks;

namespace NopLocalization.Internal
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="region">Cache region</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static T Get<T>(this ICacheManager<T> cacheManager, string key, string region, Func<T> acquire)
        {
            if (cacheManager.Exists(key, region))
                return cacheManager.Get(key, region);

            var result = acquire();

            cacheManager.Add(key, result, region);
            return result;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="region">Cache region</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static async Task<T> GetAsync<T>(this ICacheManager<T> cacheManager, string key, string region, Func<Task<T>> acquire)
        {
            if (cacheManager.Exists(key, region))
                return cacheManager.Get(key, region);

            var result = await acquire();

            cacheManager.Add(key, result, region);
            return result;
        }
    }
}

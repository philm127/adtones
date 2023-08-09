using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace EFMVC.Web.Core
{
    public class InMemoryCache : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, TimeSpan expiresIn) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.Add(expiresIn));
            }
            return item;
        }

        public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, TimeSpan expiresIn) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = await getItemCallback();
                if(item != null)
                    MemoryCache.Default.Add(cacheKey, item, DateTime.Now.Add(expiresIn));
            }
            return item;
        }
    }

    public interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, TimeSpan expiresIn) where T : class;
        Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, TimeSpan expiresIn) where T : class;
    }
}
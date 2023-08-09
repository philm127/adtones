using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model.Entities;

namespace EFMVC.Data.Repositories
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency GetDisplayCurrencyCodeForUser(int userId);
        Task<Currency> GetDisplayCurrencyCodeForUserAsync(int userId);
        Currency GetCurrencyUsingCountryId(int? countryId);
        Currency GetCurrencyUsingCurrencyId(int? currencyId);
        Task<Currency> GetCurrencyUsingCountryIdAsync(int? countryId);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId);
    }

    public static class CurrencyDefaults 
    {
        public const string DefaultCurrencySymbol = "$";
        public const string DefaultCurrencyCode = "USD";
    }

    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        private readonly Dictionary<int, Currency> _countryidToCur = new Dictionary<int, Currency>();
        private readonly Dictionary<int, Currency> _curIdToCur = new Dictionary<int, Currency>();
        private readonly Dictionary<int, Currency> _userIdToCur = new Dictionary<int, Currency>();

        /// <summary>
        /// Returns Currency for specified User.
        /// If user has preferred Currency code selected on a web page - it will be used, otherwise will be user User's original Country currency.
        /// </summary>
        /// <param name="userId">UserId for which to lookup currency.</param>
        /// <returns>Currency to be used for conversion.</returns>
        public Currency GetDisplayCurrencyCodeForUser(int userId)
        {
            Currency result;
            if (_userIdToCur.TryGetValue(userId, out result))
                return result;

            using (EFMVCDataContex db = new EFMVCDataContex())
            {
                var contact = db.Contacts.FirstOrDefault(c => c.UserId == userId);
                if (contact?.CurrencyId.HasValue ?? false)
                {
                    result = GetCurrencyUsingCurrencyId(contact.CurrencyId);
                    _userIdToCur[userId] = result;
                    return result;
                }
                result = GetCurrencyUsingCountryId(contact?.CountryId);
                _userIdToCur[userId] = result;
                return result;
            }
        }

        /// <summary>
        /// Returns Currency for specified User. Async implementation.
        /// If user has preferred Currency code selected on a web page - it will be used, otherwise will be user User's original Country currency.
        /// </summary>
        /// <param name="userId">UserId for which to lookup currency.</param>
        /// <returns>Currency to be used for conversion.</returns>
        public async Task<Currency> GetDisplayCurrencyCodeForUserAsync(int userId)
        {
            Currency result;
            if (_userIdToCur.TryGetValue(userId, out result))
                return result;

            using (EFMVCDataContex db = new EFMVCDataContex())
            {
                var contact = await db.Contacts.FirstOrDefaultAsync(c => c.UserId == userId);
                if (contact?.CurrencyId.HasValue ?? false)
                {
                    result = await GetCurrencyUsingCurrencyIdAsync(contact.CurrencyId);
                    _userIdToCur[userId] = result;
                    return result;
                }
                result = await GetCurrencyUsingCountryIdAsync(contact?.CountryId);
                _userIdToCur[userId] = result;
                return result;
            }
        }

        public Currency GetCurrencyUsingCountryId(int? countryId)
        {
            if (!countryId.HasValue)
                return GetCurrencyFromEitherCacheOrDb(db => db.Currency.FirstOrDefault(c => c.CurrencyCode == CurrencyDefaults.DefaultCurrencyCode), 0, _countryidToCur);
            return GetCurrencyFromEitherCacheOrDb(db => db.Currency.FirstOrDefault(s => s.CountryId == countryId.Value), countryId.Value, _countryidToCur);
        }

        public Currency GetCurrencyUsingCurrencyId(int? currencyId)
        {
            if (!currencyId.HasValue)
                return GetCurrencyFromEitherCacheOrDb(db => db.Currency.FirstOrDefault(c => c.CurrencyCode == CurrencyDefaults.DefaultCurrencyCode), 0, _curIdToCur);
            return GetCurrencyFromEitherCacheOrDb(db => db.Currency.FirstOrDefault(s => s.CurrencyId == currencyId.Value), currencyId.Value, _curIdToCur);
        }

        public async Task<Currency> GetCurrencyUsingCountryIdAsync(int? countryId)
        {
            if (!countryId.HasValue)
                return await GetCurrencyFromEitherCacheOrDbAsync(db => db.Currency.FirstOrDefaultAsync(c => c.CurrencyCode == CurrencyDefaults.DefaultCurrencyCode),0, _countryidToCur);
            return await GetCurrencyFromEitherCacheOrDbAsync(db => db.Currency.FirstOrDefaultAsync(s => s.CountryId == countryId), countryId.Value, _countryidToCur);
        }

        public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId)
        {
            if (!currencyId.HasValue)
                return await GetCurrencyFromEitherCacheOrDbAsync(db => db.Currency.FirstOrDefaultAsync(c => c.CurrencyCode == CurrencyDefaults.DefaultCurrencyCode), 0, _curIdToCur);
            return await GetCurrencyFromEitherCacheOrDbAsync(db => db.Currency.FirstOrDefaultAsync(s => s.CurrencyId == currencyId.Value), currencyId.Value, _curIdToCur);
        }

        private Currency GetCurrencyFromEitherCacheOrDb(Func<EFMVCDataContex, Currency> currencyPullTask, int lookup, Dictionary<int, Currency> cacheSource)
        {
            Dictionary<int, Currency> cache = cacheSource;
            Currency result;
            if (!cache.TryGetValue(lookup, out result))
            {
                result = GetCurrencyFromDb(currencyPullTask);
                cache[lookup] = result;
            }

            return result;
        }

        private async Task<Currency> GetCurrencyFromEitherCacheOrDbAsync(Func<EFMVCDataContex, Task<Currency>> currencyPullTask, int lookup, Dictionary<int, Currency> cacheSource)
        {
            Dictionary<int, Currency> cache = cacheSource;
            Currency result;
            if (!cache.TryGetValue(lookup, out result))
            {
                result = await GetCurrencyFromDbAsync(currencyPullTask);
                cache[lookup] = result;
            }

            return result;
        }

        private Currency GetCurrencyFromDb(Func<EFMVCDataContex, Currency> currencyPullTask)
        {
            using (EFMVCDataContex db = new EFMVCDataContex())
            {
                return currencyPullTask(db);
            }
        }

        private async Task<Currency> GetCurrencyFromDbAsync(Func<EFMVCDataContex, Task<Currency>> currencyPullTask)
        {
            using (EFMVCDataContex db = new EFMVCDataContex())
            {
                return await currencyPullTask(db);
            }
        }
    }
}

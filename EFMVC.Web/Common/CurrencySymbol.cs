using System;
using EFMVC.Data.Repositories;

namespace EFMVC.Web.Common
{
    public class CurrencySymbol
    {
        public string GetCurrencySymbolByCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return CurrencyDefaults.DefaultCurrencySymbol;

            switch (currencyCode)
            {
                case "GBP": return "£";
                case "USD": return "$";
                case "XOF": return "XOF";
                case "EUR": return "€";
                case "KES": return "KES";
                default: return CurrencyDefaults.DefaultCurrencySymbol;
            }
        }

        [Obsolete("Please use ICurrencyRepository")]
        public string GetCurrencySymbolusingCurrencyId(int? currencyId, ICurrencyRepository repository)
        {
            return GetCurrencySymbolByCurrencyCode(repository.GetCurrencyUsingCurrencyId(currencyId)?.CurrencyCode);
        }

        [Obsolete("Please use ICurrencyRepository")]
        public string GetCurrencySymbolusingCountryId(int? countryId, ICurrencyRepository repository)
        {
            return GetCurrencySymbolByCurrencyCode(repository.GetCurrencyUsingCountryId(countryId)?.CurrencyCode);
        }
    }
}
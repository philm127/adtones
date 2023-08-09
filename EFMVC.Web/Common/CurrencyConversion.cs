using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Adtones.Rollups.Data.Services;
using EFMVC.Data.Repositories;
using EFMVC.Web.Core.Extensions;

namespace EFMVC.Web.Common
{
    public class CurrencyModel
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }

        public decimal Convert(decimal value)
        {
            return value * Amount;
        }
    }

    public class CurrencyConversion : ICurrencyConverter
    {
        private readonly ICurrencyRepository _repository;
        static string url = ConfigurationManager.AppSettings["CurrencyUrl"];
        private readonly Dictionary<string, decimal> _cachedRates =
            new Dictionary<string, decimal>();

        public CurrencyConversion(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public Model.Entities.Currency DisplayCurrency { get; private set; }

        public void Initialize(int userId)
        {
            DisplayCurrency = _repository.GetDisplayCurrencyCodeForUser(userId);
        }

        public async Task InitializeAsync(int userId)
        {
            DisplayCurrency = await _repository.GetDisplayCurrencyCodeForUserAsync(userId);
        }

        public static CurrencyConversion CreateForCurrentUser(Controller controller, ICurrencyRepository currencyRepository)
        {
            CurrencyConversion conversion = new CurrencyConversion(currencyRepository);
            conversion.Initialize(System.Web.HttpContext.Current.User.GetEFMVCUser().UserId);
            return conversion;
        }

        public static async Task<CurrencyConversion> CreateForCurrentUserAsync(Controller controller, ICurrencyRepository currencyRepository)
        {
            CurrencyConversion conversion = new CurrencyConversion(currencyRepository);
            await conversion.InitializeAsync(System.Web.HttpContext.Current.User.GetEFMVCUser().UserId);
            return conversion;
        }

        [Obsolete]
        public CurrencyModel ForeignCurrencyConversion(string amount, string currencyFrom, string currencyTo)
        {
            decimal dAmount;
            if (!decimal.TryParse(amount, out dAmount))
                return new CurrencyModel {Amount = 0.00M, Code = "FAIL", Message = "Failed to parse amount"};
            return new CurrencyModel {Amount = Convert(dAmount, currencyFrom, currencyTo), Message = string.Empty, Code = "OK"};
        }

        public decimal Convert(decimal value, string currencyFrom, string currencyTo)
        {
            decimal rate;
            string key = $"{currencyFrom}~{currencyTo}";
            if (!_cachedRates.TryGetValue(key, out rate))
            {
                rate = CallForRate(currencyFrom, currencyTo);
                _cachedRates[key] = rate;
            }

            return Math.Round(rate * value, 2, MidpointRounding.AwayFromZero);
        }

        public decimal ConvertToDisplayCurrency(decimal value, string currencyFrom)
        {
            return Convert(value, currencyFrom, DisplayCurrency.CurrencyCode);
        }

        public decimal ConvertFromDisplayCurrency(decimal value, string currencyTo)
        {
            return Convert(value, DisplayCurrency.CurrencyCode, currencyTo);
        }

        private decimal CallForRate(string currencyFrom, string currencyTo)
        { 
            try
            {
                var param = new Currency { Amount = 1M, From = currencyFrom, To = currencyTo };
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<decimal>(response.Content);
                return InvalidRate;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Failed to get Currency rate. Error: {ex}");
                return InvalidRate;
            }
        }

        private const decimal InvalidRate = 0M;

        private class Currency
        {
            public decimal Amount { get; set; }
            public string From { get; set; }
            public string To { get; set; }
        }

        public int DisplayCurrencyId => DisplayCurrency.CurrencyId;
        public decimal ConvertToDisplay(decimal value, int fromCurrencyId)
        {
            var fromCurrency = _repository.GetCurrencyUsingCurrencyId(fromCurrencyId).CurrencyCode;
            return ConvertToDisplayCurrency(value, fromCurrency);
        }
    }

    public static class CurrencyConversionExtensions 
    {
        public static decimal Convert(this decimal value, CurrencyConversion conversion, string from, string to)
        {
            if (string.Equals(from, to))
                return value;
            return conversion.Convert(value, from, to);
        }

        public static decimal ConvertToDisplay(this decimal value, CurrencyConversion conversion, string from)
        {
            if (string.Equals(conversion.DisplayCurrency.CurrencyCode, from))
                return value;
            return conversion.ConvertToDisplayCurrency(value, from);
        }

        public static decimal ConvertFromDisplay(this decimal value, CurrencyConversion conversion, string to)
        {
            if (string.Equals(conversion.DisplayCurrency.CurrencyCode, to))
                return value;
            return conversion.ConvertFromDisplayCurrency(value, to);
        }
    }
}
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.CurrencyRate;
using EFMVC.Web.Common;
using EFMVC.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class CurrencyRateController : Controller
    {
        /// <summary>
        /// The currency rate repository
        /// </summary>
        private readonly ICurrencyRateRepository _currencyRateRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public CurrencyRateController(ICurrencyRateRepository currencyRateRepository, ICommandBus commandBus)
        {
            _currencyRateRepository = currencyRateRepository;
            _commandBus = commandBus;
        }

        private const string urlPattern = "https://data.fixer.io/api/latest?base={0}&access_key=8419e8495173e620f16c2a3b98aa2dee";
        // GET: CurrencyRate
        public ActionResult Index()
        {
            string currency = "USD";
            try
            {
                string url = string.Format(urlPattern, currency);

                using (var wc = new WebClient())
                {
                    var response = wc.DownloadString(url);

                    var currencyRate = JsonConvert.DeserializeObject<CurrencyRateModel>(response);

                    if (currencyRate.success == true)
                    {
                        Type t = currencyRate.rates.GetType();
                        foreach (PropertyInfo pi in t.GetProperties())
                        {
                            var val = pi.GetValue(currencyRate.rates, null);
                            var name = pi.Name;

                            var command = new CreateOrUpdateCurrencyRateCommand
                            {
                                CurrencyCode = name,
                                CurrencyRateAmount = Convert.ToDecimal(val)
                            };

                            ICommandResult commandResult = _commandBus.Submit(command);
                        }
                    }
                    else
                    {
                        string email = ConfigurationManager.AppSettings["SiteEmailAddress"];
                        LiveAgent.CreateTicket("Fixer api error", currencyRate.error.code + " - " + currencyRate.error.info, email);
                    }

                    ViewBag.Error = "Suceess";
                    ViewBag.InnerMsg = "No InnerException";
                    ViewBag.Time = DateTime.Now;

                    ViewBag.Window = "True";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                // ViewBag.InnerMsg = ex.InnerException.Message;
                ViewBag.ExecuteNonQuery = "No";
            }
            return View();
        }
        
    }
}
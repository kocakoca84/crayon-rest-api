using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ExchangeRate.Models;
using ExchangeRate.Services;

namespace ExchangeRate.Controllers
{
    public class ExchangeController : ApiController
    {
        private const string _uri = "https://api.exchangeratesapi.io/";
        private readonly ExchangeCalculationsHandler _exchangeCalculationsHandler = new ExchangeCalculationsHandler();
        private readonly ExternalApiHandler _externalApiHandler = new ExternalApiHandler(_uri);

        public Rate Get([FromUri] string[] dates, string currencyFrom, string currencyTo)
        {
            // TODO: handle request - extract data
            // TODO: for each date, create new request to external API
            // TODO: handle response of each
            // TODO: sort and calculate min, max, average
            // TODO: return response

            List<decimal> rates = new List<decimal>();
            foreach (var date in dates)
            {
                rates.Add(_externalApiHandler.GetExchangeRateOnDate(date, currencyFrom, currencyTo));
            }

            return _exchangeCalculationsHandler.GetNewRate(rates);
        }
    }
}

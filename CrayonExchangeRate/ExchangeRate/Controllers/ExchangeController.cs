using ExchangeRate.Models;
using ExchangeRate.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace ExchangeRate.Controllers
{
    public class ExchangeController : ApiController
    {
        private const string _uri = "https://api.exchangeratesapi.io/";
        private readonly ExchangeCalculationsHandler _exchangeCalculationsHandler = new ExchangeCalculationsHandler();
        private readonly ExternalApiHandler _externalApiHandler = new ExternalApiHandler(_uri);

        public Rate Get([FromUri] string[] dates, string currencyFrom, string currencyTo)
        {
            List<decimal> rates = new List<decimal>();
            foreach (var date in dates)
            {
                rates.Add(_externalApiHandler.GetExchangeRateOnDate(date, currencyFrom, currencyTo));
            }

            return _exchangeCalculationsHandler.GetNewRate(rates);
        }
    }
}
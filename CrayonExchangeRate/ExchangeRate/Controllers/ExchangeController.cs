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
        private readonly ExchangeCalculationsHandler _exchangeCalculationsHandler = new ExchangeCalculationsHandler();

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
                rates.Add(GetResponseFromExternalApi(date, currencyFrom, currencyTo));
            }

            return _exchangeCalculationsHandler.GetNewRate(rates);
        }

        public Rate GetNewRate(List<decimal> rates)
        {
            decimal minimum = rates.Min();
            decimal maximum = rates.Max();
            decimal sum = 0;
            foreach (var rate in rates)
            {
                sum += rate;
            }
            decimal average = Math.Round(sum / rates.Count, 12);

            return new Rate
            {
                MinimumRate = minimum,
                MaximumRate = maximum,
                AverageRate = average
            };
        }

        private decimal GetResponseFromExternalApi(string date, string currencyFrom, string currencyTo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string test = $"history?start_at={date}&end_at={date}&symbols={currencyFrom}&base={currencyTo}";
                    HttpResponseMessage response = client.GetAsync(test).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        ExpandoObject expandedJson = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
                        dynamic rates = (IDictionary<string, object>)expandedJson.FirstOrDefault(x => x.Key == "rates").Value;
                        foreach (var rate in rates)
                        {
                            var exchangeDates = (IDictionary<string, object>)rate.Value;
                            foreach (var exchangeDate in exchangeDates)
                            {
                                return Convert.ToDecimal(exchangeDate.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                var exceptionMessage = ex.Message;
                return 0;
            }

            return 0;
        }
    }
}

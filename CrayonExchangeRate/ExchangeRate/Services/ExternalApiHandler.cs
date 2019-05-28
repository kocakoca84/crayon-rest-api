using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ExchangeRate.Services
{
    public class ExternalApiHandler
    {
        private readonly string _uri;
        private const string _mediaType = "application/json";

        public ExternalApiHandler(string uri)
        {
            _uri = uri;
        }

        public decimal GetExchangeRateOnDate(string date, string currencyFrom, string currencyTo)
        {
            return GetResponseFromExternalApi<decimal>($"history?start_at={date}&end_at={date}&symbols={currencyTo}&base={currencyFrom}");
        }

        private T GetResponseFromExternalApi<T>(string request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
                    HttpResponseMessage response = client.GetAsync(request).Result;
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
                                return (T)Convert.ChangeType(exchangeDate.Value, typeof(T));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                var exceptionMessage = ex.Message;
                return (T)Convert.ChangeType(0, typeof(T));
            }

            return (T)Convert.ChangeType(0, typeof(T));
        }
    }
}
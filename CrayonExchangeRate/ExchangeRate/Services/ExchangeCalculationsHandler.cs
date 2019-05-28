using ExchangeRate.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRate.Services
{
    public class ExchangeCalculationsHandler
    {
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
    }
}
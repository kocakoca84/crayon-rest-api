# Exchange rate RESTful API
Building and running the solution:

1. Clone the repository to local machine
2. Open CrayonExchangeRate.sln with Visual Studio 2017.
3. Build the solution
4. Run with F5 or Ctrl+F5
5. Open Postman or equivalent tool and enter following GET request:
http://localhost:57865/api/exchange?dates=2018-02-01&dates=2018-02-15&dates=2018-03-01&currencyFrom=NOK&currencyTo=SEK
6. Output should be a JSON object:
{
    "MinimumRate": 0.9546869595,
    "MaximumRate": 0.9815486993,
    "AverageRate": 0.970839476467
}

# CurrencyConverterController

The `CurrencyConverterController` class provides endpoints for retrieving the latest exchange rates, converting currency values, and fetching historical exchange rate data. All endpoints require authorization with the "RequireAdministratorRole" policy.

## Endpoints

### Get Latest Exchange Rate

**URL:** `GET /api/currencyconverter/latestrate`

**Description:** Retrieves the latest exchange rate for the specified base currency.

**Parameters:**
- `basecurrency` (string, required): The base currency code (e.g., "USD").

**Responses:**
- `200 OK`: Returns the latest exchange rates as a dictionary of currency codes and their corresponding rates.
- `404 Not Found`: If the information for the specified base currency is not available.
- `400 Bad Request`: If the request is invalid.

**Example Request:**
GET /api/currencyconverter/latestrate?basecurrency=USD

**Example Response:**
{ "EUR": 0.85, "GBP": 0.75, "JPY": 110.0 }



### Convert Currency

**URL:** `GET /api/currencyconverter/convertcurrency`

**Description:** Converts a specified amount from the base currency to the target currency.

**Parameters:**
- `currencyValue` (decimal, required): The amount to be converted.
- `baseCurrency` (string, required): The base currency code (e.g., "USD").
- `targetCurrency` (string, required): The target currency code (e.g., "EUR").

**Responses:**
- `200 OK`: Returns the converted amount.
- `404 Not Found`: If the information for the specified base currency is not available.
- `400 Bad Request`: If the base or target currency is one of the excluded currencies ("TRY", "PLN", "THB", "MXN").

**Example Request:**
GET /api/currencyconverter/convertcurrency?currencyValue=100&baseCurrency=USD&targetCurrency=EUR

**Example Response:**
85.0


### Get Historical Exchange Rate Data

**URL:** `GET /api/currencyconverter/historicaldata`

**Description:** Retrieves historical exchange rate data for the specified base currency between the given start and end dates.

**Parameters:**
- `baseCurrency` (string, required): The base currency code (e.g., "USD").
- `startDate` (DateTime, required): The start date for the historical data (e.g., "2023-01-01").
- `endDate` (DateTime, required): The end date for the historical data (e.g., "2023-01-31").

**Responses:**
- `200 OK`: Returns the historical exchange rates as a dictionary of dates and their corresponding rates.
- `404 Not Found`: If the information for the specified base currency is not available.
- `400 Bad Request`: If the request is invalid.

**Example Request:**
GET /api/currencyconverter/historicaldata?baseCurrency=USD&startDate=2023-01-01&endDate=2023-01-31

**Example Response:**
{ "2023-01-01": { "EUR": 0.85, "GBP": 0.75, "JPY": 110.0 }, "2023-01-02": { "EUR": 0.86, "GBP": 0.76, "JPY": 111.0 } }


## Notes

- All endpoints are rate-limited according to the specified policy.
- Ensure that the base and target currencies are not in the excluded list ("TRY", "PLN", "THB", "MXN") for the `convertcurrency` endpoint.
- The controller logs information about each request, including the endpoint name, method, status code, and response time.

This help file provides a comprehensive overview of the `CurrencyConverterController` endpoints, their parameters, and expected responses.

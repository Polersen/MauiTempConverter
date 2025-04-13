using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTempConverter
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<decimal> GetExchangeRateAsync()
        {
            var lastFetched = Preferences.Get("lastFetched", DateTime.MinValue);

            //Using cached data to avoid straining api
            if ((DateTime.Now - lastFetched).TotalHours < 24)
            {
                return Convert.ToDecimal(Preferences.Get("cachedSEKRate", "0"));
            }

            try
            {
                string url = "https://open.er-api.com/v6/latest/USD";

                var response = await _httpClient.GetStringAsync(url);

                var exchangeRate = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

                var sekRate = exchangeRate.rates["SEK"];

                Preferences.Set("cachedSEKRate", sekRate.ToString());//Storing only SEK rate to keep cached data small
                Preferences.Set("lastFetched", DateTime.Now);

                return sekRate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem med att hämta datan: {ex.Message}");
                throw;
            }
        }
    }

    public class ExchangeRateResponse
    {
        public Dictionary<string, decimal> rates { get; set; }
    }
}

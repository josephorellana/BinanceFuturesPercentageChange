using BinanceFuturesAlert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Dao
{
    /// <summary>
    /// Access logic for ExchangeIfo model
    /// </summary>
    public class ExchangeInfoDao
    {
        private const string URL = @"https://fapi.binance.com/fapi/v1/exchangeInfo";


        /// <summary>
        /// Get symbol information from an API
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Symbol>> SymbolsAsync()
        {
            ExchangeInfo apiResponse = new ExchangeInfo();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(URL);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();

                        apiResponse = JsonSerializer.Deserialize<ExchangeInfo>(jsonString);
                    }
                    else
                    {
                       // todo
                    }
                }
                catch (Exception ex)
                {
                    // todo
                }
            }

            return apiResponse.Symbols;
        }
    }
}

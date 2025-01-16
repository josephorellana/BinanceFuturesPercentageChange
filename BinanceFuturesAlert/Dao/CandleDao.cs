using BinanceFuturesAlert.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Dao
{
    /// <summary>
    /// Access logic for Candle model
    /// </summary>
    public class CandleDao
    {
        private const string BASEURL = @"https://fapi.binance.com/fapi/v1/continuousKlines";


        /// <summary>
        /// Get a list of Candles from an API
        /// </summary>
        /// <param name="pair">Pair of cryptos</param>
        /// <param name="contractType">Contract type</param>
        /// <param name="interval">Candle interval</param>
        /// <param name="limit">Number of candles</param>
        /// <returns>Task</returns>
        public static async Task<List<Candle>> CandlesAsync(string pair, string contractType, string interval, int limit)
        {
            string url = BASEURL + "?pair="+pair+"&contractType="+contractType+"&interval="+interval+"&limit="+limit;
            var candles = new List<Candle>();
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);

                    if (!String.IsNullOrEmpty(response))
                    {

                        List<List<Object>> candlesticksData = JsonSerializer.Deserialize<List<List<Object>>>(response);

                        foreach(var data in candlesticksData)
                        {
                            try
                            {
                                Candle candle = new Candle();
                                candle.OpenTime = long.Parse(data[0].ToString());
                                candle.Open = Decimal.Parse(data[1].ToString(), CultureInfo.InvariantCulture);
                                candle.High = Decimal.Parse(data[2].ToString(), CultureInfo.InvariantCulture);
                                candle.Low = Decimal.Parse(data[3].ToString(), CultureInfo.InvariantCulture);
                                candle.Close = Decimal.Parse(data[4].ToString(), CultureInfo.InvariantCulture);
                                candle.CloseTime = long.Parse(data[6].ToString());
                                candles.Add(candle);
                            }
                            catch (Exception ex)
                            {
                                // todo
                            }
                        }
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

            return candles;
        }
    }
}

using BinanceFuturesAlert.Dao;
using BinanceFuturesAlert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;            

namespace BinanceFuturesAlert.Services
{
    /// <summary>
    /// Bussiness logic for Candlestick model
    /// </summary>
    public class CandlestickService
    {
        private static List<Candlestick> candlesticks = null;

        /// <summary>
        /// Get a list of Candlesticks.
        /// Evaluate that each crypto has a perpetual contract type, is enabled for trading,the crypto has USDT as its counterpart and the crypto is diferent from USDC
        /// </summary>
        /// <returns>Task</returns>
        public async Task<List<Candlestick>> GetCandlesticksAsync()
        {
            if (candlesticks == null)
            {
                candlesticks = new List<Candlestick>();
                var symbols = await ExchangeInfoDao.SymbolsAsync();
                foreach (var symbol in symbols)
                {
                    if (symbol.ContractType.Equals("PERPETUAL") && symbol.Pair.Contains("USDT") && symbol.Status.Equals("TRADING") && !symbol.Pair.Equals("USDCUSDT"))
                    {
                        var candlestick = new Candlestick() { Pair = symbol.Pair, ContractType = "PERPETUAL" };
                        candlesticks.Add(candlestick);
                    }
                }
            }
            
            return candlesticks;
        }


        /// <summary>
        /// Updates the candles on the candlestick
        /// </summary>
        /// <param name="candlestick">Candlestick for update</param>
        /// <param name="interval">Candle interval</param>
        /// <param name="limit">Number of candles</param>
        /// <returns></returns>
        public async Task<Candlestick> UpdateCandlestickAsync(Candlestick candlestick, string interval, int limit)
        {
            CandleService candleService = new CandleService();
            candlestick.Candles = await candleService.GetCandlesAsync(candlestick.Pair, candlestick.ContractType, interval, limit);

            return candlestick;
        }
    }
}

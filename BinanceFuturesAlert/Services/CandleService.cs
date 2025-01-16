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
    /// Bussiness logic for Candle model
    /// </summary>
    public class CandleService
    {
        /// <summary>
        /// Get a list of candles from dao
        /// </summary>
        /// <param name="pair">Pair of cryptos</param>
        /// <param name="contractType">Contract type</param>
        /// <param name="interval">Candle interval</param>
        /// <param name="limit">Number of candles</param>
        /// <returns>Task</returns>
        public async Task<List<Candle>> GetCandlesAsync(string pair, string contractType, string interval, int limit)
        {
            List<Candle>  candles = await CandleDao.CandlesAsync(pair, contractType, interval, limit);
            return candles;
        }
    }
}

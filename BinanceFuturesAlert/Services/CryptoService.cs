using BinanceFuturesAlert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Services
{
    /// <summary>
    /// Bussiness logic for Crypto model
    /// </summary>
    public class CryptoService
    {
        /// <summary>
        /// Get the crypto and calculates its percentage changes
        /// </summary>
        /// <param name="pair">Pair of cryptos</param>
        /// <param name="candles">Candles from candlestick</param>
        /// <returns>List</returns>
        public Crypto GetCrypto(string pair, List<Candle> candles)
        {
            Crypto crypto = new Crypto();
            if (candles != null && candles.Count > 0)
            {
                decimal maxValue = candles.Max(c => c.High);
                decimal minValue = candles.Min(c => c.Low);
                Candle lastCandle = candles.Last();

                if (maxValue != lastCandle.High && minValue != lastCandle.Low)
                {
                    int index;
                    index = candles.FindLastIndex(c => c.High == maxValue);
                    crypto.PercentageChangeDown = PercentageChangeBetweenValues(maxValue, lastCandle.Low);
                    crypto.MinutesChangeDown = candles.Count - index;
                    index = candles.FindLastIndex(c => c.Low == minValue);
                    crypto.PercentageChangeUp = PercentageChangeBetweenValues(minValue, lastCandle.High);
                    crypto.MinutesChangeUp = candles.Count - index;
                }
                else if (maxValue == lastCandle.High)
                {
                    int index = candles.FindIndex(c => c.High == maxValue);
                    
                    if (index == candles.Count - 1)
                    {
                        crypto.PercentageChangeUp = PercentageChangeBetweenValues(minValue, lastCandle.High);
                        index = candles.FindLastIndex(c => c.Low == minValue);
                        crypto.MinutesChangeUp = candles.Count - index;
                    }
                    else
                    {
                        return GetCrypto(pair, candles.Skip(index + 1).ToList());
                    }
                }
                else
                {
                    int index = candles.FindIndex(c => c.Low == minValue);

                    if (index == candles.Count -1)
                    {
                        crypto.PercentageChangeDown = PercentageChangeBetweenValues(maxValue, lastCandle.Low);
                        index = candles.FindLastIndex(c => c.High == maxValue);
                        crypto.MinutesChangeDown = candles.Count - index;
                    }
                    else
                    {
                        return GetCrypto(pair, candles.Skip(index + 1).ToList());
                    }
                }
            }
            

            return crypto;
        }


        /// <summary>
        /// Get the percentage change between two values
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>Double</returns>
        private double PercentageChangeBetweenValues(decimal value1,  decimal value2)
        {
            decimal change = (value2 * 100m / value1) - 100m;
            return Math.Round(Decimal.ToDouble(change), 2);
        }
    }
}

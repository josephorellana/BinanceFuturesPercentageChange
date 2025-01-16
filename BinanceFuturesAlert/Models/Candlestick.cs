using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Models
{
    public class Candlestick
    {
        public string? Pair { get; set; }
        public List<Candle> Candles { get; set; } = [];
        public string? ContractType { get; set; }
        public Crypto? Crypto { get; set; }


        public Candlestick Clone()
        {
            Crypto crypto = (this.Crypto == null) ? null : this.Crypto.Clone();
            return new Candlestick
            {
                Pair = this.Pair,
                ContractType = this.ContractType,
                Crypto = crypto, 
                Candles = []
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Models
{
    public class Candle
    {
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public long OpenTime { get; set; }
        public long CloseTime { get; set; }
    }
}

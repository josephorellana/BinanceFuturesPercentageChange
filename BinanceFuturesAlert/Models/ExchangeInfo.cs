using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Models
{
    public class ExchangeInfo
    {
        [JsonPropertyName("symbols")]
        public List<Symbol>? Symbols { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Models
{
    public class Symbol
    {
        [JsonPropertyName("pair")]
        public string Pair { get; set; }
        [JsonPropertyName("contractType")]
        public string ContractType { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}

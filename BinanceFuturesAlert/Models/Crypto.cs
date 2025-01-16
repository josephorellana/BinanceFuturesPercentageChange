using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesAlert.Models
{
    public class Crypto
    {
        //public string ? Pair { get; set; }
        public double PercentageChangeUp { get; set; }
        public int MinutesChangeUp { get; set; }
        public double PercentageChangeDown { get; set; }
        public int MinutesChangeDown { get; set; }
        public double MaxPercentageChange
        {
            get
            {
                return (PercentageChangeUp > Math.Abs(PercentageChangeDown)) ? PercentageChangeUp : Math.Abs(PercentageChangeDown);
            }
        }

        public Crypto Clone()
        {
            return new Crypto()
            {
                PercentageChangeUp = this.PercentageChangeUp,
                MinutesChangeUp = this.MinutesChangeUp,
                PercentageChangeDown = this.PercentageChangeDown,
                MinutesChangeDown = this.MinutesChangeDown
            };
        }
    }
}

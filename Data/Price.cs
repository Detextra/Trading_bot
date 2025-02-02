using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot_WPF.Data
{
    internal class Price
    {
        public string Ticker;
        public string Date;
        public string Time;
        public decimal PriceValue;

        public Price(string Ticker, string Date, string Time, decimal PriceValue)
        {

            this.Ticker = Ticker;
            this.Date = Date;
            this.Time = Time;
            this.PriceValue = PriceValue;
        }
    }
}

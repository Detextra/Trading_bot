using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot.Data
{
    internal class OHCLData
    {
        public string Ticker;
        public string Date;
        public string Time;
        public decimal OpenPrice;
        public decimal HighPrice;
        public decimal LowPrice;
        public decimal ClosePrice;

        public OHCLData(string Ticker, string Date, string Time, decimal OpenPrice, decimal HighPrice, decimal LowPrice, decimal ClosePrice) { 
        
            this.Ticker = Ticker;
            this.Date = Date;
            this.Time = Time;
            this.OpenPrice = OpenPrice;
            this.HighPrice = HighPrice;
            this.LowPrice = LowPrice;
            this.ClosePrice = ClosePrice;
        }
    }
}

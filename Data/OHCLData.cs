using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot_WPF.Data
{
    // If OhclData are accessed frequently but their fields don't change often, consider converting them to structs. Structs are stored on the stack and reduce heap allocations, improving performance in high-throughput scenarios.

    internal class OhclData
    {
        public string Ticker;
        public string Date;
        public string Time;
        public decimal OpenPrice;
        public decimal HighPrice;
        public decimal LowPrice;
        public decimal ClosePrice;
        public decimal CurrentPrice;

        public OhclData(string Ticker, string Date, string Time, decimal OpenPrice, decimal HighPrice,
            decimal LowPrice, decimal ClosePrice, decimal currentPrice)
        {

            this.Ticker = Ticker;
            this.Date = Date;
            this.Time = Time;
            this.OpenPrice = OpenPrice;
            this.HighPrice = HighPrice;
            this.LowPrice = LowPrice;
            this.ClosePrice = ClosePrice;
            CurrentPrice = currentPrice;
        }

        public static OhclData beforeFirstTick()
        {
            return new OhclData("", "", "", -1, -1, -1, -1, -1);
        }
    }
}

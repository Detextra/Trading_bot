﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Data;

namespace Trading_bot.Central
{
    internal class OhclList
    {
        public List<OhclData> OhclDatas { get; private set; }
        public int OhclTimeFrame;
        public OhclData currentOhcl {  get; private set; }

        public OhclList(int OhclTimeFrame) {
            this.OhclTimeFrame = OhclTimeFrame;
            OhclDatas = new List<OhclData>();
            currentOhcl = OhclData.beforeFirstTick();
        }

        public void Add (Price price)
        {
            currentOhcl.ClosePrice = currentOhcl.CurrentPrice;
            OhclDatas.Add(currentOhcl);
            currentOhcl = new OhclData(price.Ticker, price.Date, price.Time, price.PriceValue, price.PriceValue, price.PriceValue, price.PriceValue, price.PriceValue);
        }

        public void PrintOhclList ()
        {
            foreach (OhclData o in OhclDatas)
            {
                Console.WriteLine("" + o.Ticker + "\t" + o.Date + "\t" + o.Time + "\t"
                    + o.OpenPrice + "\t" + o.HighPrice + "\t" + o.LowPrice + "\t" + o.ClosePrice);
            }
        }
    }
}

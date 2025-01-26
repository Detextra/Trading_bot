using OxyPlot.Series;
using OxyPlot;
using OxyPlot.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Data;
using Trading_bot.Market;
using Trading_bot.Strategy;
using Trading_bot.Strategy.StrategyType;
using System.Windows.Forms; // Add this import for Form and Controls

namespace Trading_bot.Risk
{
    internal class RiskModule
    {
        public decimal cash;
        public List<Tuple<string, RiskData>> riskDatas;
        public List<OrderLimit> ordersLimit;

        private Form parentForm;

        public RiskModule(decimal cash)
        {
            this.cash = cash;
            riskDatas = new List<Tuple<string, RiskData>>();
            ordersLimit = new List<OrderLimit>();
        }

        public void OnPriceReceived(object sender, Price price)
        {
            decimal actualCash = cash;
            foreach (OrderLimit orderLimit in ordersLimit)
            {
                actualCash -= orderLimit.Price * orderLimit.Quantity;
            }
            RiskData riskData = new RiskData();
            riskData.value = actualCash;
            riskDatas.Add(Tuple.Create(price.Date + "" + price.Time, riskData));
        }

        public void ProcessOrderSold(OrderLimit orderLimit)
        {
            // the cash is not substracted when asset is bought for the order => add the buying and solding price to OrderLimit
            cash += orderLimit.Price * orderLimit.Quantity;
            ordersLimit.Remove(orderLimit);
        }

        public void PlotRiskData()
        {
            var model = new PlotModel { Title = "Risk Data over Time" };

            var series = new LineSeries
            {
                Title = "Risk Value",
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerType = MarkerType.Circle
            };

            foreach (var data in riskDatas)
            {
                DateTime time = DateTime.ParseExact(data.Item1, "yyyyMMddHHmmss", null);
                decimal value = data.Item2.value;

                series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(time), (double)value));
            }

            model.Series.Add(series);

            // Save the plot to a PNG file
            var pngExporter = new PngExporter(600, 400, 96);
            var filePath = "./../../../../RiskDataPlot.png";
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    pngExporter.Export(model, fileStream);
                }
                Console.WriteLine($"Plot successfully saved as '{filePath}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving plot: {ex.Message}");
            }
        }
    }

    internal class RiskData
    {
        public decimal value;

        public RiskData() { }

        public RiskData(decimal value)
        {
            this.value = value;
        }
    }
}
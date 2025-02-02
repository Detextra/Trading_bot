using System.Collections.Generic;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace Trading_bot_WPF
{
    public partial class GraphWindow : Window
    {
        public GraphWindow(List<DataPoint> dataPoints, string title)
        {
            InitializeComponent();

            var model = new PlotModel { Title = title };
            var series = new LineSeries { Title = "Risk Data" };

            foreach (var point in dataPoints)
            {
                series.Points.Add(point);
            }

            model.Series.Add(series);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Risk" });

            PlotView.Model = model;
        }
    }
}

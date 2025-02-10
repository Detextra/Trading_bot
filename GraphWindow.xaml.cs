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

            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                StringFormat = "MM/dd/yy",
                IntervalType = DateTimeIntervalType.Days,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Risk"
            });

            PlotView.Model = model;
        }
    }
}

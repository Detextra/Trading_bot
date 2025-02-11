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
        public GraphWindow(List<DataPoint> perfDatas, List<DataPoint> marketPrices, string title)
        {
            InitializeComponent();

            var model = new PlotModel { Title = title };

            // downsizing perfDatas
            int step = Math.Max(1, perfDatas.Count / 5000); // Reduce to ~5000 points max
            var downsampledPerfDatas = perfDatas.Where((x, i) => i % step == 0).ToList();
            Console.WriteLine($"Original: {perfDatas.Count}, Downsampled: {downsampledPerfDatas.Count}");

            var series = new LineSeries
            {
                Title = "Risk/Perf Data",
                StrokeThickness = 1,
                MarkerType = MarkerType.Circle,
                Color = OxyColors.Blue,
                RenderInLegend = true,
                ItemsSource = downsampledPerfDatas,
                YAxisKey = "Perf"

            };

            model.Series.Add(series);

            var marketSeries = new LineSeries
            {
                Title = "Market Prices",
                StrokeThickness = 2,
                MarkerType = MarkerType.Square,
                Color = OxyColors.Red,
                RenderInLegend = true,
                ItemsSource = marketPrices,
                YAxisKey = "Market"

            };

            model.Series.Add(marketSeries);

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
                Key = "Perf",
                Position = AxisPosition.Left,
                Title = "Perf",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash
            });

            model.Axes.Add(new LinearAxis
            {
                Key = "Market",
                Position = AxisPosition.Right,
                Title = "Market Prices",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash
            });

            PlotView.Model = model;
        }
    }
}

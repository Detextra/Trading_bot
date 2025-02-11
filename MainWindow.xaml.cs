using OxyPlot;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trading_bot_WPF;

namespace Trading_bot_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TradingManager tradingManager;

        public MainWindow()
        {
            InitializeComponent();
            tradingManager = new TradingManager();

            // Subscribe to the GraphDataReady event
            tradingManager.GraphDataReady += OnGraphDataReady;

            // Run the trading process (will trigger graph display)
            tradingManager.RunTrading();
        }

        // This method is called when graph data is ready
        private void OnGraphDataReady(List<DataPoint> perfDatas, List<DataPoint> marketPrices, string strategyName)
        {
            // Create a new GraphWindow for each strategy's graph
            var graphWindow = new GraphWindow(perfDatas, marketPrices, strategyName);
            graphWindow.Show();
        }
    }
}
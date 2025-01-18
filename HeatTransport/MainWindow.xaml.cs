using System.Collections.Generic;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace HeatTransport
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Draw(List<double> x, List<double> y)
        {
            if (x.Count != y.Count)
            {
                throw new System.ArgumentException("X and Y lists must have the same length.");
            }

            // Setting data series
            var series = new LineSeries
            {
                Values = new ChartValues<double>(y),
                Title = "Function",
                PointGeometry = null
            };

            // Setting X axis
            chart.AxisX.Add(new Axis
            {
                Title = "X",
                Labels = x.ConvertAll(v => v.ToString("0.00"))
            });

            // Setting Y axis
            chart.AxisY.Add(new Axis
            {
                Title = "Y"
            });

            // Painting the series to the graph
            chart.Series = new SeriesCollection { series };
        }
    }
}

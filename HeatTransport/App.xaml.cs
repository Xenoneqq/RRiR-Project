using System.Collections.Generic;
using System.Windows;

namespace HeatTransport
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();

            int elements = 6;
            Calculator calculator = new Calculator();
            var result = calculator.Calculate(elements);
            ResultPrinter resultPrint = new ResultPrinter();

            List<double> x = result.Item1;
            List<double> y = result.Item2;
            resultPrint.SaveToFile(x,y);

            window.Draw(x, y);
            window.Show();
        }
    }
}

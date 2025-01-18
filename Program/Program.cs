using HeatTransport;

Calculator calculator = new Calculator();
var result = calculator.Calculate(6);

List<double> x = result.Item1;
List<double> y = result.Item2;

ResultPrinter resultPrint = new ResultPrinter();
resultPrint.SaveToFile(x,y);
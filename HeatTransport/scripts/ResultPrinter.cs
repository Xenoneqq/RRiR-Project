using System;
using System.Collections.Generic;
using System.IO;

namespace HeatTransport
{
    public class ResultPrinter
    {
        public void SaveToFile(List<double> x, List<double> y, string filePath = "wyniki.txt")
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < x.Count; i++)
                {
                    writer.WriteLine($"{x[i]}; {y[i]}");
                }
            }

            Console.WriteLine($"Wyniki zapisane do pliku: {filePath}");
        }
    }
}


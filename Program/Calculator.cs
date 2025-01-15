using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Integration;

namespace HeatTransport
{
    public class Calculator
    {
        private readonly int integrationPointCount = 30;
        private readonly double integrationAccuracy = 1e-12;
        private double h;
        private int elements;

        public void Solve(int elements)
        {
            this.elements = elements;
            this.h = 2.0 / elements;

            // Macierz równań
            var equationMatrix = BuildEquationMatrix();

            // Wektor wyników
            var resultVector = BuildResultVector();

            // Rozwiązywanie układu równań
            var coefficients = equationMatrix.Solve(resultVector);

            // Dodanie wartości brzegowej
            var y = new List<double>(coefficients.ToArray()) { 0.0 };
            var x = BuildXList();

            // Wyświetlanie wyników
            PrintResults(equationMatrix, resultVector, x, y);
        }

        private Matrix<double> BuildEquationMatrix()
        {
            var equationMatrix = Matrix<double>.Build.Dense(elements, elements);

            for (int i = 0; i < elements; i++)
            {
                for (int j = 0; j < elements; j++)
                {
                    if (i == j)
                    {
                        equationMatrix[i, j] = BuildDiagonalElement(i);
                    }
                    else if (Math.Abs(i - j) == 1)
                    {
                        equationMatrix[i, j] = BuildOffDiagonalElement(i, j);
                    }
                    else
                    {
                        equationMatrix[i, j] = 0.0;
                    }
                }
            }

            return equationMatrix;
        }

        private double BuildDiagonalElement(int i)
        {
            double a = 2.0 * Math.Max(0.0, (i - 1.0) / elements);
            double b = 2.0 * Math.Min(1.0, (i + 1.0) / elements);
            return Integral(i, i, a, b);
        }

        private double BuildOffDiagonalElement(int i, int j)
        {
            double a = 2.0 * Math.Max(0.0, 1.0 * Math.Min(i, j) / elements);
            double b = 2.0 * Math.Min(1.0, 1.0 * Math.Max(i, j) / elements);
            return Integral(i, j, a, b);
        }

        // Liczenie całki metodą Gaussa Legendre
        private double Integral(int i, int j, double a, double b)
        {
            var result = GaussLegendreRule.Integrate(
                x => k(x) * ePrim(i, x) * ePrim(j, x),
                a,
                b,
                integrationPointCount
            );
            return result - (e(i, 0) * e(j, 0));
        }

        private Vector<double> BuildResultVector()
        {
            var resultVector = Vector<double>.Build.Dense(elements);
            for (int i = 0; i < elements - 1; i++)
            {
                resultVector[i] = L(i);
            }

            // Brzegowy warunek Dirichleta
            resultVector[elements - 1] = 3.0;

            return resultVector;
        }

        private List<double> BuildXList()
        {
            var x = new List<double>();
            for (int i = 0; i <= elements; i++)
            {
                x.Add(h * i);
            }
            return x;
        }

        private double L(int i)
        {
            return -20.0 * e(i, 0);
        }

        private double k(double x)
        {
            if (x >= 0 && x <= 1) return 1.0;
            if (x > 1 && x <= 2) return 2.0;
            throw new ArgumentOutOfRangeException(nameof(x), "Wartość poza zakresem.");
        }

        private double e(int i, double x)
        {
            if (x < h * (i - 1) || x > h * (i + 1))
                return 0;

            if (x < h * i) return x / h - i + 1;
            return -x / h + i + 1;
        }

        private double ePrim(int i, double x)
        {
            if (x < h * (i - 1) || x > h * (i + 1))
                return 0;

            if (x < h * i)
                return 1 / h;

            return -1 / h;
        }

        private void PrintResults(Matrix<double> equationMatrix, Vector<double> resultVector, List<double> x, List<double> y)
        {
            Console.WriteLine("> Matrix <");
            PrintMatrix(equationMatrix);

            Console.WriteLine("\n> Result Vector <");
            PrintVector(resultVector);

            Console.WriteLine("\nValues of x:");
            Console.WriteLine(string.Join(", ", x));

            Console.WriteLine("\nValues of y:");
            Console.WriteLine(string.Join(", ", y));
        }

        private void PrintMatrix(Matrix<double> matrix)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    Console.Write($"{Math.Round(matrix[i, j], 2)} ");
                }
                Console.WriteLine();
            }
        }

        private void PrintVector(Vector<double> vector)
        {
            foreach (var value in vector)
            {
                Console.Write($"{Math.Round(value, 2)} ");
            }
            Console.WriteLine();
        }
    }
}

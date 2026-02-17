using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using OxyPlot;
using OxyPlot.Series;

namespace ИСИП323_Эрдынеева_Суранзан
{
    public partial class ThirdWindow : Window
    {
        private const double B = 35.4;
        private const double X0 = 1.75;
        private const double XK = -2.5;
        private const double DX = -0.25;

        public PlotModel PlotModel { get; set; }

        public ThirdWindow()
        {
            InitializeComponent();

            BackButton3.Click += BackButton3_Click;
            ResetButton3.Click += ResetButton3_Click;
            CalculateButton3.Click += CalculateButton3_Click;

            PlotModel = new PlotModel
            {};

            PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "X",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Y",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            DataContext = this;
        }

        private void CalculateButton3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                double userX = X0;
                if (!string.IsNullOrWhiteSpace(xValue3.Text))
                {
                    if (!double.TryParse(xValue3.Text, out userX))
                    {
                        MessageBox.Show("Некорректное значение x", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                var results = CalculateFunctionOnInterval();

                DisplayResults(results);

                PlotFunction(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fatal error: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<(double X, double Y)> CalculateFunctionOnInterval()
        {
            var results = new List<(double X, double Y)>();

            double step = DX;
            double currentX = X0;

            int steps = (int)((XK - X0) / DX) + 1;
            if (steps < 0) steps = -steps + 1;

            for (int i = 0; i < steps; i++)
            {
                try
                {
                    double y = CalculateFunction(currentX);
                    results.Add((currentX, y));
                }
                catch (Exception ex)
                {
                    results.Add((currentX, double.NaN));
                }

                currentX += DX;

                if ((DX > 0 && currentX > XK + 0.001) || (DX < 0 && currentX < XK - 0.001))
                    break;
            }
            return results;
        }

        private double CalculateFunction(double x)
        {
            double arg = Math.Abs(x - B);
            if (arg <= 0)
            {
                throw new ArgumentException("Вне области определения (логарифм)");
            }

            double term1 = 0.001 * Math.Pow(Math.Abs(x), 5/2);
            double term2 = Math.Log(arg);

            return term1 + term2;
        }

        private void DisplayResults(List<(double X, double Y)> results)
        {
            string output = "X\t\tY\r\n";
            output += "------------------------\r\n";

            foreach (var result in results)
            {
                if (double.IsNaN(result.Y))
                {
                    output += $"{result.X:F4}\tне определен\r\n";
                }
                else
                {
                    output += $"{result.X:F4}\t{result.Y:F6}\r\n";
                }
            }

            ResultBox3.Text = output;
        }

        private void PlotFunction(List<(double X, double Y)> results)
        {
            PlotModel.Series.Clear();
            var functionSeries = new LineSeries
            {
                Title = "y = 10⁻³|x|⁵/² + ln|x - b|",
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.None
            };

            foreach (var result in results)
            {
                if (!double.IsNaN(result.Y))
                {
                    functionSeries.Points.Add(new DataPoint(result.X, result.Y));
                }
            }

            PlotModel.Series.Add(functionSeries);
            PlotView.InvalidatePlot(true);
        }

        private void ResetButton3_Click(object sender, RoutedEventArgs e)
        {
            xValue3.Text = "";
            ResultBox3.Text = "";

            PlotModel.Series.Clear();
            PlotView.InvalidatePlot(true);
        }

        private void BackButton3_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow();
            secondWindow.Show();
            this.Close();
        }
    }
}
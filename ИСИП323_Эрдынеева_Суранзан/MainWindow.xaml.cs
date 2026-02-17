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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ИСИП323_Эрдынеева_Суранзан
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CalculateButton.Click += CalculateButton_Click;
            ResetButton.Click += ResetButton_Click;
            NextButton.Click += NextButton_Click;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x = double.Parse(xValue.Text);
                double y = double.Parse(yValue.Text);
                double z = double.Parse(zValue.Text);

                double result = CalculateFunction1(x, y, z);

                ResultBox.Text = result.ToString("F4"); // округление до 4-х знаков
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка ввода - не числа", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fatal error: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double CalculateFunction1(double x, double y, double z)
        {
            double p1 = Math.Pow(x, y + 1) + Math.Exp(y - 1); // (x^(y+1) + e^(y-1))

            double tgz = Math.Tan(z); // tg(z)
            double denom = 1 + x * Math.Abs(y - tgz); // (1 + x * |y - tg(z)|)

            double yMinusX = Math.Abs(y - x); // |y - x|
            double p2 = 1 + yMinusX; //  (1 + |y - x|)

            double p3 = Math.Pow(yMinusX, 2) / 2; // |y - x|^2/2
            double p4 = Math.Pow(yMinusX, 3) / 3; // |y - x|^3/3

            double result = (p1 / denom) * p2 + p3 - p4;

            return result;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            xValue.Text = "";
            yValue.Text = "";
            zValue.Text = "";
            ResultBox.Text = "0";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
            SecondWindow secondWindow = new SecondWindow();
            secondWindow.Show();

            this.Close();
        }
    }
}
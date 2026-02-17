using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ИСИП323_Эрдынеева_Суранзан
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();

            CalculateButton2.Click += CalculateButton2_Click;
            ResetButton2.Click += ResetButton2_Click;
            BackButton2.Click += BackButton2_Click;
            NextButton2.Click += NextButton2_Click;
        }

        private void CalculateButton2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xValue2.Text) ||
                    string.IsNullOrWhiteSpace(iValue2.Text))
                {
                    MessageBox.Show("Не заполнены все значения", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Парсим значения
                if (!double.TryParse(xValue2.Text, out double x))
                {
                    MessageBox.Show("Некорректный ввод x", "Ошибка");
                    return;
                }

                if (!double.TryParse(iValue2.Text, out double i))
                {
                    MessageBox.Show("Некорректный ввод i", "Ошибка");
                    return;
                }

                // выбор функции
                double fx = 0;
                if (FuncOption_shx.IsChecked == true)
                {
                    fx = Math.Sinh(x); 
                }
                else if (FuncOption_x2.IsChecked == true)
                {
                    fx = x * x;
                }
                else if (FuncOption_e2.IsChecked == true)
                {
                    fx = Math.Exp(2);
                }
                else
                {
                    MessageBox.Show("Не выбрана функция", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double result = CalculateE(i, x, fx);

                ResultBox2.Text = result.ToString("F4");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double CalculateE(double i, double x, double fx)
        {
            
            bool isEven = i % 2 == 0;

            if (!isEven && x > 0) // i нечетное и x > 0
            {
                if (fx < 0)
                {
                    throw new Exception("Вне области определения функции - квадратный корень от отрицательного");
                }
                return i * Math.Sqrt(fx);
            }
            else if (isEven && x < 0) // i четное и x < 0
            {
                
                double absFx = Math.Abs(fx);
                if (absFx < 1e-10)
                {
                    throw new Exception("Вне области определения функции - деление на 0");
                }
                return i / (2 * Math.Sqrt(absFx));
            }
            else 
            {
                double absProduct = Math.Abs(i * fx);
                return Math.Sqrt(absProduct);
            }
        }

        private void ResetButton2_Click(object sender, RoutedEventArgs e)
        {
            
            xValue2.Text = "";
            iValue2.Text = "";
            ResultBox2.Text = "";

            FuncOption_shx.IsChecked = false;
            FuncOption_x2.IsChecked = false;
            FuncOption_e2.IsChecked = false;
        }

        private void BackButton2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void NextButton2_Click(object sender, RoutedEventArgs e)
        {
            // Переходим к третьему окну
            ThirdWindow thirdWindow = new ThirdWindow();
            thirdWindow.Show();
            this.Close();
        }
    }
}

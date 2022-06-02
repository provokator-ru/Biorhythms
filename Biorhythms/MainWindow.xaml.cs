using LiveCharts;
using LiveCharts.Wpf;
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

namespace Biorhythms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Labels = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Arbitraryvalue.IsEnabled = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Arbitraryvalue.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Inf_columns> biorhythms = new List<Inf_columns>();
            int arbitrarys = 0;
            DateTime birthdayDate = DateTime.Now;

            const int phys = 23;
            const int emot = 28;
            const int intel = 33;

            try
            {
                birthdayDate = Convert.ToDateTime(Birthdaydate.Text);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при получении даты рождения");
            }

            if (Arbitraryvalue.IsEnabled == true)
            {
                try
                {
                    arbitrarys = Convert.ToInt32(Arbitraryvalue.Text);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при получении данных отчета");
                }
            }
            else
            {
                try
                {
                    arbitrarys = Convert.ToInt32(Prognoz.Text);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при получении данных отчета");
                }
            }

            DateTime datePrognoz= Convert.ToDateTime(Date1.Text); ;
            for (int i = 0; i < arbitrarys; i++)
            {
                datePrognoz = Convert.ToDateTime(Date1.Text);
                var bior = new Inf_columns()
                {
                    Date = datePrognoz.AddDays(i).ToShortDateString(),
                    Emotional = Math.Round((Math.Sin(2 * Math.PI * ((datePrognoz - birthdayDate).Days + i) / emot)) * 100, 2),
                    Intellectual = Math.Round((Math.Sin(2 * Math.PI * ((datePrognoz - birthdayDate).Days + i) / intel)) * 100, 2),
                    Physical = Math.Round((Math.Sin(2 * Math.PI * ((datePrognoz - birthdayDate).Days + i) / phys)) * 100, 2),
                };
                bior.Total = Math.Round(bior.Emotional + bior.Intellectual + bior.Physical, 2);

                biorhythms.Add(bior);
            }
            Dates.ItemsSource = biorhythms;

            double maxEm = double.MinValue;
            double maxInt = double.MinValue;
            double maxPhys = double.MinValue;
            double maxSum = double.MinValue;
            string maxEmDate = String.Empty;
            string maxIntDate = String.Empty;
            string maxPhysDate = String.Empty;
            string maxSumDate = String.Empty;

            foreach (Inf_columns bior in biorhythms)
            {
                if (bior.Physical > maxPhys)
                {
                    maxPhys = bior.Physical;
                    maxPhysDate = bior.Date;
                }
                if (bior.Emotional > maxEm)
                {
                    maxEm = bior.Emotional;
                    maxEmDate = bior.Date;
                }
                if (bior.Intellectual > maxInt)
                {
                    maxInt = bior.Intellectual;
                    maxIntDate = bior.Date;
                }
                if (bior.Total > maxSum)
                {
                    maxSum = bior.Total;
                    maxSumDate = bior.Date;
                }
            }


            list.Items.Clear();
            list.Items.Add($"Дата рождения - {birthdayDate.ToShortDateString()}");
            list.Items.Add($"Длительность прогноза - {arbitrarys}");
            list.Items.Add($"Период с {datePrognoz.ToShortDateString()} по {datePrognoz.AddDays(arbitrarys).ToShortDateString()}");
            list.Items.Add($"Эмоциональный максимум - {maxEm}");
            list.Items.Add($"Интеллектуальный максимум - {maxInt}");
            list.Items.Add($"Физический максимум - {maxPhys}");

            ChartValues<double> PhysicalValues = new ChartValues<double>();
            ChartValues<double> EmotionalValues = new ChartValues<double>();
            ChartValues<double> IntellectualValues = new ChartValues<double>();
            SeriesCollection series = new SeriesCollection();

            foreach (Inf_columns bior in biorhythms)
            {
                PhysicalValues.Add(bior.Physical);
                EmotionalValues.Add(bior.Emotional);
                IntellectualValues.Add(bior.Intellectual);
                Labels.Add(bior.Date.ToString());
            }

            series.Add(new LineSeries
            {
                Title = "Физические ритмы",
                Values = PhysicalValues,
            });
            series.Add(new LineSeries
            {
                Title = "Эмоциональные ритмы",
                Values = EmotionalValues
            });
            series.Add(new LineSeries
            {
                Title = "Интеллектуальные ритмы",
                Values = IntellectualValues
            });

            graph.AxisY = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Значения",
                    MinValue = -100,
                    MaxValue = 100,
                }
            };
            graph.Series = series;
            graph.Update();

            graph.AxisX = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Дата",
                    MinValue = 0,
                    Labels = Labels,
                }
            };
        }
    }
}

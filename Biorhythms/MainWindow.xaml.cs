﻿using System;
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
            DateTime birthDate = DateTime.Now;

            const int phys = 23;
            const int emot = 28;
            const int intel = 33;

            try
            {
                birthDate = Convert.ToDateTime(Birthdaydate.Text);
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
            DateTime dateCountDown;
            for (int i = 0; i < arbitrarys; i++)
            {
                dateCountDown = Convert.ToDateTime(Date1.Text);
                var bior = new Inf_columns()
                {
                    Date = dateCountDown.AddDays(i).ToShortDateString(),
                    Emotional = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / emot)) * 100, 2),
                    Intellectual = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / intel)) * 100, 2),
                    Physical = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / phys)) * 100, 2),
                };
                bior.Total = Math.Round(bior.Emotional + bior.Intellectual + bior.Physical, 2);

                biorhythms.Add(bior);
            }
            Dates.ItemsSource = biorhythms;
        }
    }
}

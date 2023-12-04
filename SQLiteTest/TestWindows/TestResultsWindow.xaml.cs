using LiveCharts;
using Microsoft.Win32;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Models;
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

namespace SQLiteTest.TestWindows
{
    /// <summary>
    /// Логика взаимодействия для TestResultsWindow.xaml
    /// </summary>
    public partial class TestResultsWindow : Window
    {
        private User _user;
        private ITestResult _result;
        public SeriesCollection _SeriesCollection { get; set; }
        private ITestChartValues _chartValues;

        public TestResultsWindow(ITestResult result, User user, ITestChartValues testChart)
        {
            InitializeComponent();
            Loaded += TestResultsWindow_Loaded;
            _user = user;
            _result = result;
            _chartValues = testChart;
        }

        public Func<double, string> YFormatter { get; set; }
        private async void TestResultsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainTextBlock.Text = _result.GetResults();
            processedResultsBlock.Text = _result.GetProccesedResults();

            if (_chartValues != null)
            {
                chartName.Text = _chartValues.GetChartName();
                _SeriesCollection = _chartValues.GetSeriesCollection();
                NoChartBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoChartBox.Text = "Для данных результатов теста график не предусмотрен";
                mainChart.Visibility = Visibility.Collapsed;
            }

            YFormatter = value => value.ToString("C");
            DataContext = this;

            await Task.Run(() => _result.SaveToDatabase(_user));
        }

        private void saveToExcel_Click(object sender, RoutedEventArgs e)
        {
            var filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Title = "Сохранить резульаты";
            saveFileDialog.Filter = "Файл Excel|*.xls;*.xlsx;*.xlsm";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CheckPathExists = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            if (filePath != string.Empty)
            {
                _result.SaveToExcel(filePath);
            }
        }
    }
}

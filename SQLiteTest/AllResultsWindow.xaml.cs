using Microsoft.Win32;
using SQLiteTest.Data;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
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

namespace SQLiteTest
{
    /// <summary>
    /// Логика взаимодействия для AllResultsWindow.xaml
    /// </summary>
    public partial class AllResultsWindow : Window
    {
        private IDatabaseResults _databaseResults;
        private List<DBMunsterbergResult> _munsterbergResults;
        private List<DBShulteResult> _shulteResults;
        private List<DBStrupResult> _strupResults;
        private List<User> _users;
        public AllResultsWindow()
        {
            InitializeComponent();
            _databaseResults = new DBResultsRepository(new DatabaseContext());
            _users = _databaseResults.GetAllUsers().ToList();
            _munsterbergResults = _databaseResults.GetAllMunsterbergResults().ToList();
            _shulteResults = _databaseResults.GetAllShulteResults().ToList();
            _strupResults = _databaseResults.GetAllStrupResults().ToList();

            SetGridViewCollumns();

            munsterbergListView.ItemsSource = _munsterbergResults;
            strupListView.ItemsSource = _strupResults;
            shulteListView.ItemsSource = _shulteResults;
        }

        private void SetGridViewCollumns()
        {
            CreateGridViewColumn(munsterbergGridView, "NumderOfAllWords", "Всего слов");
            CreateGridViewColumn(munsterbergGridView, "NumberOfCorrectWords", "Павильно введены");
            CreateGridViewColumn(munsterbergGridView, "NumberOfMisstakenWords", "Неправильно введены");
            CreateGridViewColumn(munsterbergGridView, "AllTime", "Время");
            CreateGridViewColumn(munsterbergGridView, "User", "Пользователь");

            CreateGridViewColumn(shulteGridView, "AllTime", "Время прохождения");
            CreateGridViewColumn(shulteGridView, "User", "Пользователь");

            CreateGridViewColumn(strupGridView, "NumberOfCorrectItem", "Верно");
            CreateGridViewColumn(strupGridView, "NumberOfMisstake", "Неверно");
            CreateGridViewColumn(strupGridView, "MinTimePt1", "Минимальное время 1ч");
            CreateGridViewColumn(strupGridView, "MinTimePt2", "Минимальное время 2ч");
            CreateGridViewColumn(strupGridView, "MaxTimePt1", "Максимальное время 1ч");
            CreateGridViewColumn(strupGridView, "MaxTimePt2", "Максимальное время 2ч");
            CreateGridViewColumn(strupGridView, "AverageTimePt1", "Среднее время 1ч");
            CreateGridViewColumn(strupGridView, "AverageTimePt2", "Среднее время 2ч");
            CreateGridViewColumn(strupGridView, "DelayTime", "Задержка");
            CreateGridViewColumn(strupGridView, "User", "Пользователь");

        }

        private void CreateGridViewColumn(GridView gridView, string path, string header)
        {
            GridViewColumn column = new GridViewColumn();
            column.DisplayMemberBinding = new Binding(path);
            column.Header = header;
            gridView.Columns.Add(column);
        }

        private void ButtonToExcel_Click(object sender, RoutedEventArgs e)
        {
            IDataToExcel dataToExcel = null;

            switch (tabControl.SelectedIndex)
            {
                case 0: dataToExcel = new MunsterbergDBToExcel(_munsterbergResults); break;
                case 1: dataToExcel = new StrupDBToExcel(_strupResults); break;
                case 2: dataToExcel = new ShulteDBToExcel(_shulteResults); break;
                default:
                    break;
            }

            if (dataToExcel != null)
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
                    dataToExcel.SaveToExcel(filePath);
                }
            }
            else
            {
                MessageBox.Show("Ошибка индекса вкладки, сохранение невозможно");
            }
        }
    }
}

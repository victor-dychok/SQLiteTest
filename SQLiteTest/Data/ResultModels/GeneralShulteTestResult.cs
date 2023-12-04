using Aspose.Cells;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
using SQLiteTest.Models;
using SQLiteTest.TestModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace SQLiteTest.Data.ResultModels
{
    public class GeneralShulteTestResult : ITestResult
    {
        private List<ShulteTestResult> _shulteResults;
        private DatabaseContext _appContext;
        private Workbook _workbook = new Workbook();

        public GeneralShulteTestResult(List<ShulteTestResult> shulteResults, DatabaseContext appContext)
        {
            _shulteResults = shulteResults;
            _appContext = appContext;
        }

        public ITestChartValues GetChartValuesInterface()
        {
            return new ShulteChart(_shulteResults);
        }

        public string GetProccesedResults()
        {
            string result = "";
            ShulteTestItem incorrect;
            int i = 1;
            foreach (ShulteTestResult itemList in _shulteResults)
            {
                incorrect = itemList.GetFirstIncorrectItem();
                result += $"Часть {i++}\n";
                if (itemList.GetFirstIncorrectItem() != null)
                {
                    result += $"Первый неверно введенный {incorrect}\n";
                }
                else
                {
                    result += "Всё верно\n";
                }
            }
            return result;
        }

        public string GetResults()
        {
            string result = "";
            int i = 1;
            foreach (ShulteTestResult itemList in _shulteResults)
            {
                result += $"\nЧасть {i++}\n";
                foreach (var item in itemList.GetResults())
                {
                    result += $"{item}\n";
                }
            }
            return result;
        }


        public void SaveToDatabase(User user)
        {
            IDatabaseResults database = new DBResultsRepository(_appContext);
            int incorrectNumber = 0;
            string incorrectColor = null;
            foreach (ShulteTestResult item in _shulteResults)
            {
                ShulteTestItem incorrect = item.GetFirstIncorrectItem();
                if (incorrect != null)
                {
                    incorrectNumber = incorrect.GetNumber();
                    incorrectColor = incorrect.GetColorName();
                }

                var DBResult = new DBShulteResult()
                {
                    FirstIncorrectNumber = incorrectNumber,
                    FirstIncorrectColor = incorrectColor,
                    AllTime = item.Time,
                    User = database.GetUser(user)
                };
                database.AddShulteResult(DBResult);
            }
            database.SaveToDatabase();
        }

        public void SaveToExcel(string pass)
        {
            Worksheet worksheet = _workbook.Worksheets[0];
            ArrayList list = new ArrayList();

            string buff;

            int i = 1;
            foreach (var itemList in _shulteResults)
            {
                list.Add($"Часть №{i++}");
                foreach (var item in itemList.GetResults())
                {
                    buff = item.GetNumber() + " ";
                    buff += item.GetColor() == Colors.Red ? "красный" : "черный";
                    list.Add(buff);
                }
                list.Add("Время (сек): " + itemList.Time);

                ShulteTestItem incorrect = itemList.GetFirstIncorrectItem();
                if (incorrect != null)
                {
                    buff = incorrect.GetNumber() + " ";
                    buff += incorrect.GetColor() == Colors.Red ? "красный" : "черный";
                    list.Add("Первый ошибочно введенный: " + buff);
                }
                else
                {
                    list.Add("Ошибок нет");
                }
            }
            try
            {
                worksheet.Cells.ImportArrayList(list, 0, 0, true);
                _workbook.Save(pass);
                MessageBox.Show($"Файл результатов успешно сохранен.\nРасположение файла: {pass}");
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить файл");
            }
        }
    }
}


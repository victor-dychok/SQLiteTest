using Aspose.Cells;
using SQLiteTest.Data.ExcelModels;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
using SQLiteTest.Models;
using SQLiteTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLiteTest.Data.ResultModels
{
    public class GeneralStrupTestResult : ITestResult
    {
        List<StrupTestResults> _results;
        private int _correctItems;
        private List<int> _failedItems = new List<int>();
        private DatabaseContext _context;
        private Workbook _workbook = new Workbook();
        public GeneralStrupTestResult(List<StrupTestResults> results, DatabaseContext context)
        {
            _results = results;
            _context = context;
        }

        public string GetResults()
        {
            string result = "";
            foreach (StrupTestResults testResults in _results)
            {
                foreach (StrupTestResultItem item in testResults.Results)
                {
                    result += item.Answer + " - " + item.Expected + " : " + item.TimeInMillisec + '\n';
                }
                result += "\n";
            }
            return result;
        }

        public string GetProccesedResults()
        {
            ProccesingResults();
            string processedResult = $"Правильных ответов: {_correctItems}\n" +
                $"Неправильных ответов {_failedItems.Count()}\n";

            var minTime = new List<uint>();
            var maxTime = new List<uint>();
            int i = 1;
            foreach (var testResults in _results)
            {
                minTime.Add(testResults.GetMinItemTime());
                maxTime.Add(testResults.GetMaxItemTime());

                processedResult += $"Часть {i}:\n Минимальное время - {testResults.GetMinItemTime()}, " +
                    $"максимальное время - {testResults.GetMaxItemTime()}\n";
            }


            processedResult += $"Среднее время первой части: {_results[0].GetAvaregeTime()}мс\n" +
                $"Среднее время второй части: {_results[1].GetAvaregeTime()}мс\n" +
                $"Задержка, обусловленная эфектом Струпа: {_results[1].GetAvaregeTime() - _results[0].GetAvaregeTime()}мс";

            return processedResult;
        }

        private void ProccesingResults()
        {
            foreach (StrupTestResults testResults in _results)
            {
                for (int i = 0; i < testResults.Results.Count; ++i)
                {
                    if (testResults.Results[i].Expected == testResults.Results[i].Answer)
                    {
                        ++_correctItems;
                    }
                    else
                    {
                        _failedItems.Add(i);
                    }
                }
            }
        }

        public void SaveToDatabase(User user)
        {
            IDatabaseResults database = new DBResultsRepository(_context);
            int avTime1 = _results[0].GetAvaregeTime(), avTime2 = _results[1].GetAvaregeTime();


            DBStrupResult result = new DBStrupResult()
            {
                NumberOfCorrectItem = _correctItems,
                NumberOfMisstake = _failedItems.Count(),
                MinTimePt1 = (int)_results[0].GetMinItemTime(),
                MinTimePt2 = (int)_results[1].GetMinItemTime(),
                MaxTimePt1 = (int)_results[0].GetMaxItemTime(),
                MaxTimePt2 = (int)_results[1].GetMaxItemTime(),
                AverageTimePt1 = avTime1,
                AverageTimePt2 = avTime2,
                DelayTime = avTime2 - avTime1,
                User = database.GetUser(user)
            };
            database.AddStrupResult(result);
            database.SaveToDatabase();
        }

        public void SaveToExcel(string pass)
        {
            Worksheet worksheet = _workbook.Worksheets[0];
            List<StrupExcel> list = new List<StrupExcel>();

            foreach (StrupTestResults testResults in _results)
            {
                foreach (StrupTestResultItem item in testResults.Results)
                {
                    list.Add(new StrupExcel(item.Expected,  item.Answer == item.Expected ? "+" : "-" ,item.TimeInMillisec));
                }
            }

            try
            {
                worksheet.Cells.ImportCustomObjects(list,
                new string[] { "Answer", "Expected", "Time" }, // propertyNames
                false, // isPropertyNameShown
                0, // firstRow
                0, // firstColumn
                list.Count, // Number of objects to be exported
                true, // insertRows
                null, // dateFormatString
                false); // convertStringToNumber
                _workbook.Save(pass);
                MessageBox.Show($"Файл результатов успешно сохранен.\nРасположение файла: {pass}");
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить файл");
            }
        }

        public ITestChartValues GetChartValuesInterface()
        {
            return new StrupChart(_results);
        }
    }
}

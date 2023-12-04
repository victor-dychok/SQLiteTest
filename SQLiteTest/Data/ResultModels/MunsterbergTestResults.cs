using Aspose.Cells;
using SQLiteTest.Data.ExcelModels;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLiteTest.Data.ResultModels
{
    public class MunsterbergTestResults : ITestResult
    {
        private List<string> _userAnswers;
        private List<string> _correctTestAnswers;
        private List<string> _correctUserAnswers;
        private List<string> _incorrectUserAnswers;
        private int _numberOfCorrectAnswers;
        private DatabaseContext _context;
        private Workbook _workbook = new Workbook();

        private uint _time;

        public MunsterbergTestResults(List<string> userAnswers, List<string> correctAnswers, uint time, DatabaseContext context)
        {
            _time = time;
            _userAnswers = userAnswers;
            _correctTestAnswers = correctAnswers;
            _correctUserAnswers = new List<string>();
            _incorrectUserAnswers = new List<string>();

            _numberOfCorrectAnswers = 0;
            foreach (var word in _userAnswers)
            {
                if (_correctTestAnswers.Contains(word))
                {
                    _numberOfCorrectAnswers++;
                    _correctUserAnswers.Add(word);
                }
                else
                {
                    _incorrectUserAnswers.Add(word);
                }
            }
            _context = context;
        }

        public ITestChartValues GetChartValuesInterface()
        {
            return null;
        }

        public string GetProccesedResults()
        {
            return $"Количество правильных ответов: {_numberOfCorrectAnswers}";
        }

        public string GetResults()
        {
            string result = "Правильно указанные слова\n";

            foreach (var word in _correctUserAnswers)
            {
                result += $"{word}\n";
            }
            result += "\n";
            if (_incorrectUserAnswers.Count > 0)
            {
                result += "Неправильно указанные слова\n";
                foreach (var word in _incorrectUserAnswers)
                {
                    result += $"{word}\n";
                }
            }

            return result;
        }

        public void SaveToDatabase(User user)
        {
            IDatabaseResults database = new DBResultsRepository(_context);

            DBMunsterbergResult result = new DBMunsterbergResult()
            {
                NumberOfCorrectWords = _correctUserAnswers.Count,
                NumberOfMisstakenWords = _incorrectUserAnswers.Count,
                NumderOfAllWords = _correctTestAnswers.Count,
                AllTime = (int)_time,
                User = database.GetUser(user)
            };
            database.AddMunsterbergResult(result);
            database.SaveToDatabase();
        }

        public void SaveToExcel(string pass)
        {
            try
            {
                Worksheet worksheet = _workbook.Worksheets[0];
                List<MunsterbergExcel> list = new List<MunsterbergExcel>();
                string userAnswer = "";
                list.Add(new MunsterbergExcel("Ответы:", string.Empty));
                foreach (var item in _correctTestAnswers)
                {
                    userAnswer = _correctUserAnswers.Contains(item) ? "+" : "-";
                    list.Add(new MunsterbergExcel(item, userAnswer));
                }
                list.Add(new MunsterbergExcel("Ошибочно найденные:", string.Empty));
                foreach (var item in _incorrectUserAnswers)
                {
                    list.Add(new MunsterbergExcel(item, string.Empty));
                }

                worksheet.Cells.ImportCustomObjects(list,
                new string[] { "CorrectAnswer", "UserAnswer" }, // propertyNames
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
    }
}

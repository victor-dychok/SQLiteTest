using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.ExcelModels
{
    public class MunsterbergExcel
    {
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }

        public MunsterbergExcel(string correctAnswer, string userAnswer)
        {
            CorrectAnswer = correctAnswer;
            UserAnswer = userAnswer;
        }
    }
}

using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Interfaces
{
    public interface ITestResult
    {
        string GetResults();
        string GetProccesedResults();
        void SaveToDatabase(User user);
        void SaveToExcel(string pass);
        ITestChartValues GetChartValuesInterface();
    }
}

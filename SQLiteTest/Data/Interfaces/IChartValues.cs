using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Interfaces
{
    public interface ITestChartValues
    {
        SeriesCollection GetSeriesCollection();
        string GetChartName();
    }
}

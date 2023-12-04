using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using SQLiteTest.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteTest.Data.ResultModels;

namespace SQLiteTest.Data.Repository
{
    public class ShulteChart : ITestChartValues
    {
        private List<LineSeries> _listValues = new List<LineSeries>();
        private List<ShulteTestResult> _results = new List<ShulteTestResult>();

        public ShulteChart(List<ShulteTestResult> results)
        {
            _listValues = new List<LineSeries>();
            _results = results;

            int j = 0;
            foreach (var result in results)
            {
                var values = new ChartValues<ObservablePoint>();
                int i = 0;
                foreach (var item in result.GetTimeArray())
                {
                    values.Add(new ObservablePoint(i++, item));
                }

                string title = "";
                switch (j++)
                {
                    case 0: title = "Красные по убыванию"; break;
                    case 1: title = "Черные по возрастанию"; break;
                    case 2: title = "Оба цвета"; break;
                }

                _listValues.Add(new LineSeries
                {
                    Values = values,
                    Title = title
                });
            }
        }

        public string GetChartName()
        {
            return "Граффики времени (сек) выбора ответа для 3-ех частей теста Шулте";
        }

        public SeriesCollection GetSeriesCollection()
        {
            var collection = new SeriesCollection();
            collection.AddRange(_listValues);
            return collection;
        }
    }
}

using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Repository
{
    public class StrupChart : ITestChartValues
    {
        private List<StrupTestResults> _result;
        private List<LineSeries> _listValues = new List<LineSeries>();

        public StrupChart(List<StrupTestResults> result)
        {
            _result = result;
            int j = 0;

            foreach (var list in _result)
            {
                var values = new ChartValues<ObservablePoint>();
                int i = 0;
                foreach (var item in list.Results)
                {
                    values.Add(new ObservablePoint(i++, item.TimeInMillisec));
                }


                _listValues.Add(new LineSeries
                {
                    Values = values,
                    Title = $"Часть {++j}"
                });
            }
        }

        public string GetChartName()
        {
            return "Граффики времени (мс) прохождения теста струпа для 1-ой и 2-ой частей";
        }

        public SeriesCollection GetSeriesCollection()
        {
            var collection = new SeriesCollection();
            collection.AddRange(_listValues);
            return collection;
        }
    }
}

using SQLiteTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.ResultModels
{
    public class StrupTestResults
    {
        private int _numberOfCorrectItems;
        public int NumberOfCorrectResults { get { return _numberOfCorrectItems; } }
        private List<int> _incorrectItemsId;

        public int GetAvaregeTime()
        {
            uint time = 0;
            foreach (var result in _results)
            {
                time += result.TimeInMillisec;
            }
            return (int)time / _results.Count;
        }

        public List<int> GetIncorrectItems()
        {
            return _incorrectItemsId;
        }

        private List<StrupTestResultItem> _results;
        public List<StrupTestResultItem> Results { get { return _results; } }

        public StrupTestResults()
        {
            _results = new List<StrupTestResultItem>();
            _numberOfCorrectItems = 0;
            _incorrectItemsId = new List<int>();
        }

        public void AddResult(string result, string expected, uint time)
        {
            _results.Add(new StrupTestResultItem { Answer = result, Expected = expected, TimeInMillisec = time });
        }


        public uint GetMaxItemTime()
        {
            uint max = _results[0].TimeInMillisec;
            foreach (StrupTestResultItem item in _results)
            {
                if (item.TimeInMillisec > max)
                {
                    max = item.TimeInMillisec;
                }
            }
            return max;
        }

        public uint GetMinItemTime()
        {
            uint max = _results[0].TimeInMillisec;
            foreach (StrupTestResultItem item in _results)
            {
                if (item.TimeInMillisec < max)
                {
                    max = item.TimeInMillisec;
                }
            }
            return max;
        }
    }
}

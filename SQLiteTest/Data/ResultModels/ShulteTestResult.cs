using SQLiteTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.ResultModels
{
    public class ShulteTestResult
    {
        private List<ShulteTestItem> _itemList;
        private List<int> _timeArray;
        private ShulteTestItem _firstIncorrectItem;
        private int _timeInSec;

        public int Time
        {
            get { return _timeInSec; }
            set { _timeInSec = value; }
        }


        public ShulteTestResult()
        {
            _itemList = new List<ShulteTestItem>();
            _timeArray = new List<int>();
        }

        public void AddItem(ShulteTestItem item, int time)
        {
            _itemList.Add(item);
            _timeArray.Add(time);
        }
        public void SetFirstIncorrectItem(ShulteTestItem item)
        {
            _firstIncorrectItem = item;
        }
        public ShulteTestItem GetFirstIncorrectItem()
        {
            return _firstIncorrectItem;
        }

        public List<ShulteTestItem> GetResults()
        {
            return _itemList;
        }

        public List<int> GetTimeArray()
        {
            return _timeArray;
        }
    }
}

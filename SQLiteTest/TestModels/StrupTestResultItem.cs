using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.TestModels
{
    public class StrupTestResultItem
    {
        private string _answer;
        public string Answer
        { get { return _answer; } set { _answer = value; } }

        private string _expected;
        public string Expected { get { return _expected; } set { _expected = value; } }

        private uint _timeInMillisec;
        public uint TimeInMillisec { get { return _timeInMillisec; } set { _timeInMillisec = value; } }
    }
}

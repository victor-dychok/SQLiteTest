using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.ExcelModels
{
    public class StrupExcel
    {
        public string Answer { get; set; }
        public string Expected { get; set; }
        public uint Time { get; set; }

        public StrupExcel(string answer, string expected, uint time)
        {
            Answer = answer;
            Expected = expected;
            Time = time;
        }
    }
}

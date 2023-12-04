using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Models
{
    public class DBStrupResult
    {
        public int Id { get; set; }
        public int NumberOfCorrectItem { get; set; }
        public int NumberOfMisstake { get; set; }
        public int MinTimePt1 { get; set; }
        public int MinTimePt2 { get; set; }
        public int MaxTimePt1 { get; set; }
        public int MaxTimePt2 { get; set; }
        public int AverageTimePt1 { get; set; }
        public int AverageTimePt2 { get; set; }
        public int DelayTime { get; set; }
        public virtual User? User { get; set; }
    }
}

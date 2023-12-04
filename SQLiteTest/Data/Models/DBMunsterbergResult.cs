using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Models
{
    public class DBMunsterbergResult
    {
        public int Id { get; set; }
        public int NumderOfAllWords { get; set; }
        public int NumberOfCorrectWords { get; set; }
        public int NumberOfMisstakenWords { get; set; }
        public int AllTime { get; set; }
        public virtual User? User { get; set; }
    }
}

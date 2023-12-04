using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Models
{
    public class DBShulteResult
    {
        public int Id { get; set; }
        public int? FirstIncorrectNumber { get; set; }
        public string? FirstIncorrectColor { get; set; }
        public int AllTime { get; set; }
        public virtual User? User { get; set; }
    }
}

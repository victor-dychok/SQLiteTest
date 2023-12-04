using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Group}гр.";
        }
    }
}

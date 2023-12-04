using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLiteTest.TestModels
{
    public class ShulteTestItem
    {
        private int _number;
        Color _color;

        public int GetNumber()
        {
            return _number;
        }

        public SolidColorBrush GetColorBrush()
        {
            return new SolidColorBrush(_color);
        }

        public Color GetColor()
        {
            return _color;
        }
        public string GetColorName()
        {
            return _color == Colors.Red ? "красный" : "черный";
        }

        public ShulteTestItem(int number, Color color)
        {
            _number = number;
            _color = color;
        }

        public bool Equals(ShulteTestItem item)
        {
            return item._color == _color && item._number == _number;
        }

        public override string ToString()
        {
            return $"{GetColorName()} - {_number}";
        }
    }
}

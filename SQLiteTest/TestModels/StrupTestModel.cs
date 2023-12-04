using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLiteTest.TestModels
{
    abstract class StrupTestModel
    {
            protected List<WordWithColor> items;
            protected int size = 30;
            protected int _iterator;
            protected bool _IsGenerating;

            protected Random random;

            public StrupTestModel(bool isGenerating)
            {
                random = new Random();
                _IsGenerating = isGenerating;
                _iterator = 0;
                generateItems();
            }

            protected static WordWithColor[] availableVariants = new WordWithColor[] {
            new WordWithColor { Name = "Желтый", Color = Color.FromRgb(255, 238, 0) },
            new WordWithColor { Name = "Зелёный", Color = Colors.Green },
            new WordWithColor { Name = "Красный", Color = Colors.Red },
            new WordWithColor { Name = "Синий", Color = Colors.Blue },
            new WordWithColor { Name = "Чёрный", Color = Colors.Black }
        };

            protected abstract void generateItems();

            public string GetColorName(string color)
            {
                return availableVariants.FirstOrDefault(i => i.Color.ToString() == color).Name;
            }

            public WordWithColor GetNextItem()
            {
                if (_iterator < size)
                {
                    return items[_iterator++];
                }
                else
                {
                    return null;
                }
            }

            public int getNumberOfItems()
            {
                return availableVariants.Length;
            }

            public Color GetColorById(int id)
            {
                return availableVariants[id].Color;
            }

            public string GetWordById(int id)
            {
                return availableVariants[id].Name;
            }

            public int GetSize()
            {
                return size;
            }
        }
}

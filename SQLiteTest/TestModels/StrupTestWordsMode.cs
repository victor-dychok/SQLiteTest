using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLiteTest.TestModels
{
    internal class StrupTestWordsMode : StrupTestModel
    {
        public StrupTestWordsMode(bool isGeneratingColors) : base(isGeneratingColors) { }

        protected override void generateItems()
        {
            items = new List<WordWithColor>();
            for (int i = 0; i < size; i++)
            {
                int max = availableVariants.Length;
                Color color = _IsGenerating ?
                    availableVariants[random.Next(max)].Color : Color.FromRgb(0, 0, 0);
                items.Add(new WordWithColor
                {
                    Name = availableVariants[random.Next(max)].Name,
                    Color = color
                });
            }
        }
    }
}

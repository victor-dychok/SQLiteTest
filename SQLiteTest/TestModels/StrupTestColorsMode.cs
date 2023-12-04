using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.TestModels
{
    internal class StrupTestColorsMode : StrupTestModel
    {
        public StrupTestColorsMode(bool isGeneratingWords) : base(isGeneratingWords) { }

        protected override void generateItems()
        {
            items = new List<WordWithColor>();
            int max = availableVariants.Length;
            for (int i = 0; i < size; i++)
            {
                string word = availableVariants[random.Next(max)].Name;
                items.Add(new WordWithColor
                {
                    Name = word,
                    Color = availableVariants[random.Next(max)].Color
                });
            }
        }
    }
}

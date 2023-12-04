using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLiteTest.TestModels
{
    internal class ShulteTestModel
    {
        private string _description;

        private const int MaxItemNumberValue = 25;
        private const int GridSize = 49;

        private Random _random = new Random();
        private List<ShulteTestItem> _availableItems;
        private List<ShulteTestItem> _elementsArray;


        private List<ShulteTestItem> _redOnlyAnsw;
        private List<ShulteTestItem> _blackOnlyAnsw;
        private List<ShulteTestItem> _bouthAnsw;

        private List<ShulteTestItem> _currentAnswersArray;
        private int _currentAnswerIndex;

        public enum AnswerStarus
        {
            True,
            False,
            Done
        }
        public enum Modes
        {
            None,
            Red,
            Black,
            Bouth
        }
        private Modes _mode = Modes.None;
        public ShulteTestModel()
        {
            generateItemsList();
            generateTestArrays();

            _description = "Чтобы приступить нажмите кнопку \"Готово!\"";
        }

        public bool StartNextModeIfNotDone()
        {
            switch (_mode)
            {
                case Modes.None:
                    _currentAnswersArray = _redOnlyAnsw;
                    break;
                case Modes.Red:
                    _currentAnswersArray = _blackOnlyAnsw;
                    break;
                case Modes.Black:
                    _currentAnswersArray = _bouthAnsw;
                    break;
                case Modes.Bouth: return true;

                default:
                    break;
            }
            _currentAnswerIndex = 0;
            ++_mode;
            return false;
        }

        public AnswerStarus GetCurrentAnswer(ShulteTestItem item)
        {
            if (_currentAnswerIndex < _currentAnswersArray.Count - 1)
            {
                if (_currentAnswersArray[_currentAnswerIndex].Equals(item))
                {
                    _currentAnswerIndex++;
                    return AnswerStarus.True;
                }
                else
                    return AnswerStarus.False;
            }
            else
            {
                if (_currentAnswersArray[_currentAnswerIndex].Equals(item))
                {
                    _currentAnswerIndex++;
                    return AnswerStarus.Done;
                }
                else
                    return AnswerStarus.False;
            }
        }

        public List<ShulteTestItem> GetElemts()
        {
            return _elementsArray;
        }

        public string GetDescription()
        {
            return _description;
        }

        private void generateItemsList()
        {
            _availableItems = new List<ShulteTestItem>();
            Color color;
            for (int i = 0; i < GridSize; i++)
            {
                if (i < MaxItemNumberValue)
                    color = Colors.Red;
                else
                    color = Colors.Black;
                _availableItems.Add(new ShulteTestItem(i % MaxItemNumberValue + 1, color));
            }
        }

        private void generateTestArrays()
        {
            _elementsArray = new List<ShulteTestItem>();
            int index;
            for (int i = 0; i < GridSize; i++)
            {
                index = _random.Next(0, _availableItems.Count());
                _elementsArray.Add(_availableItems[index]);
                _availableItems.RemoveAt(index);
            }

            _blackOnlyAnsw = new List<ShulteTestItem>();
            _redOnlyAnsw = new List<ShulteTestItem>();
            _bouthAnsw = new List<ShulteTestItem>();

            _redOnlyAnsw.Add(new ShulteTestItem(MaxItemNumberValue, Colors.Red));
            _bouthAnsw.Add(new ShulteTestItem(MaxItemNumberValue, Colors.Red));

            for (int i = 1; i < MaxItemNumberValue; i++)
            {
                _bouthAnsw.Add(new ShulteTestItem(i, Colors.Black));
                _bouthAnsw.Add(new ShulteTestItem(MaxItemNumberValue - i, Colors.Red));
                _redOnlyAnsw.Add(new ShulteTestItem(MaxItemNumberValue - i, Colors.Red));
                _blackOnlyAnsw.Add(new ShulteTestItem(i, Colors.Black));
            }
        }
    }
}

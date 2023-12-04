using SQLiteTest.Data;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.ResultModels;
using SQLiteTest.Models;
using SQLiteTest.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLiteTest.TestWindows
{
    /// <summary>
    /// Логика взаимодействия для StrupTestWindow.xaml
    /// </summary>
    public partial class StrupTestWindow : Window
    {
        private StrupTestModel _strupTestModel;
        private int numberOfTry;
        private List<StrupTestResults> _results;
        private WordWithColor _currentItem;
        private DateTime _time;
        private DatabaseContext _context;
        private bool _isTestDone = true;
        private User _user;

        public string GetInstruction()
        {
            string instruction = "Тест состоит из 2-ух частей. " +
                "Перед началом каждой части необходимо нажать кнопку \"Начать тестирование\"\n";
            switch (_mode)
            {
                case StrupTestModes.ModeWords:

                    instruction += "В обоих частях теста вам будут предъявляться названия цветов.\n" +
                        "В первой части все названия цветов будут напечатаны черным цветом, " +
                        "во второй части цвет шрифта произвольный.\n" +
                        "Ваша задача: в обоих частях теста нажимать на кнопки, цвета которых соответствуют смылсу предъявляемого слова" +
                        " вне зависимости от цвета шрифта.\n";

                    break;
                case StrupTestModes.ModeColors:

                    instruction += "В первой части теста вам будут демонстрироваться прямоугольники определенных цветов, " +
                        "во второй - названия цветов, напечатанные определенным цветом.\n" +
                        "Ваша задача: в обоих частях теста нажимать на кнопки, на которых написано название цвета, " +
                        "соответствующее ЦВЕТУ стимульного материала.\n" +
                        "(т.е. во воторой части вы должны игнорировать смысл предъявляемого слова и указывать название цвета шрифта)";

                    break;
                default: break;
            }
            return instruction;
        }

        public enum StrupTestModes
        {
            ModeWords,
            ModeColors
        }
        private StrupTestModes _mode;

        public enum StrupTestPart
        {
            First,
            Second,
            Done
        }
        private StrupTestPart _testPart;


        public StrupTestWindow(StrupTestModes mode, DatabaseContext context)
        {
            InitializeComponent();
            Loaded += StrupTestWindow_Loaded;
            _context = context;
            numberOfTry = 0;
            _mode = mode;
        }

        public void SetUser(User user)
        {
            _user = user;
        }

        private void StrupTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _isTestDone = false;
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            _strupTestModel = _mode == StrupTestModes.ModeWords ?
                _strupTestModel = new StrupTestWordsMode(_testPart == StrupTestPart.Second) :
                _strupTestModel = new StrupTestColorsMode(_testPart == StrupTestPart.Second);

            if (_testPart == StrupTestPart.First)
            {
                _results = new List<StrupTestResults>();
                mainButton.IsEnabled = false;

                setButtons();
                numberOfTryText.Text = $"{++numberOfTry}/{_strupTestModel.GetSize()}";
            }
            else if (_testPart == StrupTestPart.Second)
            {
                buttonGrid.IsEnabled = true;
                mainButton.IsEnabled = false;
            }

            _results.Add(new StrupTestResults());
            _currentItem = _strupTestModel.GetNextItem();
            resetItemText(_currentItem.Name, new SolidColorBrush(_currentItem.Color));
        }

        private List<Button> createButtons()
        {
            List<Button> buttons = new List<Button>();
            for (int i = 0; i < _strupTestModel.getNumberOfItems(); i++)
            {
                var button = new Button();
                button.Margin = new Thickness(5);
                button.Click += new RoutedEventHandler(colorButton_Click);
                SolidColorBrush brush = new SolidColorBrush(_strupTestModel.GetColorById(i));

                if (_mode == StrupTestModes.ModeWords)
                {
                    button.Background =
                    button.BorderBrush =
                    button.Foreground = brush;
                }
                else if (_mode == StrupTestModes.ModeColors)
                {
                    Style style = this.FindResource("MaterialDesignOutlinedButton") as Style;
                    button.Style = style;
                    button.Content = _strupTestModel.GetWordById(i);
                }
                buttons.Add(button);
            }
            return buttons;
        }

        private void setButtons()
        {
            List<Button> buttonList = createButtons();
            int columnsNumber = 6;
            int columnSpan = 2;
            int currentColumn = 0;
            int currentRow = 0;

            for (int i = 0; i < buttonList.Count; ++i)
            {
                var button = buttonList[i];
                buttonGrid.Children.Add(button);
                Grid.SetColumn(button, currentColumn);
                Grid.SetRow(button, currentRow);
                Grid.SetColumnSpan(button, columnSpan);
                currentColumn += columnSpan;
                if (currentColumn >= columnsNumber)
                {
                    ++currentRow;
                    currentColumn = 1;
                }
            }
        }

        private void colorButton_Click(Object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string result = "", expected = "";

            if (_mode == StrupTestModes.ModeWords)
            {
                result = _strupTestModel.GetColorName(button.Background.ToString());
                expected = _currentItem.Name;
            }
            else if (_mode == StrupTestModes.ModeColors)
            {
                result = button.Content.ToString();
                expected = _strupTestModel.GetColorName(_currentItem.Color.ToString());
            }

            _results[Convert.ToInt32(_testPart)].AddResult(result, expected, Convert.ToUInt32((DateTime.UtcNow - _time).TotalMilliseconds));
            _currentItem = _strupTestModel.GetNextItem();

            if (_currentItem != null)
            {
                resetItemText(_currentItem.Name, new SolidColorBrush(_currentItem.Color));
                ++numberOfTry;
            }
            else
            {
                itemText.Text = "";
                buttonGrid.IsEnabled = false;
                numberOfTry = 1;
                mainButton.IsEnabled = true;
                _testPart++;
                if (_testPart == StrupTestPart.Done)
                {
                    var generalResult = new GeneralStrupTestResult(_results, _context);

                    TestResultsWindow testResults = new TestResultsWindow(generalResult, _user, generalResult.GetChartValuesInterface());
                    testResults.Show();

                    MessageBox.Show("Тест окончен!");
                    _isTestDone = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Первая часть завершена");
                }
            }
            numberOfTryText.Text = $"{numberOfTry}/{_strupTestModel.GetSize()}";
        }

        private async void resetItemText(string text, SolidColorBrush color)
        {
            itemText.Text = "";
            itemText.Background = new SolidColorBrush(Colors.LightGray);
            await Task.Delay(50);
            if (_mode == StrupTestModes.ModeWords)
            {
                itemText.Foreground = color;
                itemText.Text = text;
            }
            else
            {
                if (_testPart == StrupTestPart.First)
                {
                    itemText.Background = color;
                }
                else
                {
                    itemText.Text = text;
                    itemText.Background = new SolidColorBrush(Colors.LightGray);
                    itemText.Foreground = color;
                }
            }
            _time = DateTime.UtcNow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isTestDone)
            {
                MessageBoxResult mbResult = MessageBox.Show("Вы уверены, что хотите прервать прохождение теста? Результаты не будут сохранены", "Внимание", MessageBoxButton.OKCancel);
                if(mbResult != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

using SQLiteTest.Data.ResultModels;
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
using static SQLiteTest.TestModels.ShulteTestModel;
using System.Windows.Threading;
using SQLiteTest.Data;
using SQLiteTest.Models;

namespace SQLiteTest.TestWindows
{
    /// <summary>
    /// Логика взаимодействия для ShulteTestWindow.xaml
    /// </summary>
    public partial class ShulteTestWindow : Window
    {
        private const int SecMax = 300;
        private const int TableSize = 7;
        private TextBlock _notification;
        private bool IsNotificationShown = true;
        private DispatcherTimer timer;
        private int _secAll = SecMax;
        private int minuts;
        private int sec;
        private ShulteTestModel _shulteTestModel;
        private List<ShulteTestResult> _testResults;
        private DatabaseContext _context;
        private int _part = 0;
        private DateTime _currentTime;
        private User _user;

        private bool _isTestDone = true;

        private List<string> _shortInstructions = new List<string>
        {
            "Выберите все красные элементы по убыванию",
            "Выберите все черные элементы по возрастанию",
            "Выбирайте поочередно красные элеленты по убыванию\nи черные по возрастанию"
        };

        public string GetInstruction()
        {
            return "Вам будет предъявлена таблица, содержащая 25 красных и 24 черных элемента.\n" + 
                "Тест состоит из 3 частей.\n" +
                "В первой части вам необходимо выбрать все красные элементы по убыванию.\n" +
                "Во второй части вам необходимо выбрать все черные элементы по возрастанию.\n" +
                "В третьей части нужно выбирать красные и черные элементы поочередно" +
                " в той же последовательности начиная с красного. " +
                "(т.е. 25 - красный, 1 - черный, 24 - красный, 2 - черный и так далее)\n";
        }

        public ShulteTestWindow(DatabaseContext context)
        {
            InitializeComponent();
            _context = context;

            Loaded += ShulteTestWindow_Loaded;

        }

        private void ShulteTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _isTestDone = false;
            timerGrid.Opacity = 0;
            _shulteTestModel = new ShulteTestModel();
            fullfillGrid(_shulteTestModel.GetElemts());

            _testResults = new List<ShulteTestResult>();
            showNotification();
            _shulteTestModel.StartNextModeIfNotDone();
            answerStatus.Foreground = new SolidColorBrush(Colors.White);
        }

        private void dispetcerTimer_Tick(object sender, EventArgs e)
        {
            _secAll--;
            minuts = _secAll / 60;
            sec = _secAll % 60;
            timerBlock.Text = $"{minuts} мин. {sec} сек.";
            if (_secAll <= 0)
            {
                NextMode();
            }
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotificationShown)
            {
                testGrid.Children.Remove(_notification);
                _testResults.Add(new ShulteTestResult());
                IsNotificationShown = false;
                timerGrid.Opacity = 1;

                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(dispetcerTimer_Tick);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
                shortInstructionBox.Text = _shortInstructions[_part];

                _currentTime = DateTime.UtcNow;
            }
            else
            {
                NextMode();
            }
        }

        private void NextMode()
        {
            if (_shulteTestModel.StartNextModeIfNotDone())
            {
                EndOfTheTest();
            }
            else
            {
                _testResults.Add(new ShulteTestResult());
                _testResults[_part++].Time = SecMax - _secAll;
                _secAll = SecMax;
                MessageBox.Show("Переход к следующей части");
                shortInstructionBox.Text = _shortInstructions[_part];

                answerStatusBorder.Background = new SolidColorBrush(Colors.Transparent);
                answerStatus.Text = string.Empty;
            }
        }

        private void fullfillGrid(List<ShulteTestItem> elements)
        {
            for (int i = 0; i < TableSize; i++)
            {
                testGrid.ColumnDefinitions.Add(new ColumnDefinition());
                testGrid.RowDefinitions.Add(new RowDefinition());
            }

            Button gridElement;
            ShulteTestItem item;
            SolidColorBrush brush;

            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    item = elements[j + i * TableSize];
                    brush = item.GetColorBrush();
                    gridElement = new Button();
                    gridElement.Content = item.GetNumber().ToString();
                    gridElement.FontWeight = FontWeights.Bold;
                    gridElement.Background = brush;
                    gridElement.BorderBrush = brush;
                    gridElement.Margin = new Thickness(3);
                    if (brush.Color == Colors.Black)
                    {
                        gridElement.Click += blackGridButton_Click;
                    }
                    else if (brush.Color == Colors.Red)
                    {
                        gridElement.Click += redGridButton_Click;
                    }
                    gridElement.FontSize = 20;
                    gridElement.Height = gridElement.Width;

                    testGrid.Children.Add(gridElement);
                    Grid.SetColumn(gridElement, i);
                    Grid.SetRow(gridElement, j);
                }

            }
        }

        private void blackGridButton_Click(object sender, RoutedEventArgs e)
        {
            var gridElement = sender as Button;
            itemButton(Convert.ToInt32(gridElement.Content), Colors.Black);

        }

        private void redGridButton_Click(object sender, RoutedEventArgs e)
        {
            var gridElement = sender as Button;
            itemButton(Convert.ToInt32(gridElement.Content), Colors.Red);
        }

        private void itemButton(int index, Color color)
        {
            ShulteTestItem currentItem = new ShulteTestItem(index, color);
            _testResults[_part].AddItem(currentItem, Convert.ToInt32((DateTime.UtcNow - _currentTime).TotalMilliseconds));
            switch (_shulteTestModel.GetCurrentAnswer(currentItem))
            {
                case AnswerStarus.True:
                    {
                        answerStatusBorder.Background = currentItem.GetColorBrush();
                        answerStatus.Text = currentItem.GetNumber().ToString();
                    }
                    break;
                case AnswerStarus.False:
                    {
                        if (_testResults[_part].GetFirstIncorrectItem() == null)
                        {
                            _testResults[_part].SetFirstIncorrectItem(currentItem);
                        }
                    }
                    break;
                case AnswerStarus.Done:
                    {
                        NextMode();
                    }
                    break;
                default:
                    break;
            }
        }

        public void SetUser(User user)
        {
            _user = user;
        }

        private void EndOfTheTest()
        {
            MessageBox.Show("Готово");
            timer.Stop();

            GeneralShulteTestResult generalResult = new GeneralShulteTestResult(_testResults, _context);
            TestResultsWindow testResults = new TestResultsWindow(generalResult, _user, generalResult.GetChartValuesInterface());
            testResults.Show();

            _isTestDone = true;
            Close();
        }

        private void showNotification()
        {
            setNotification();

            testGrid.Children.Add(_notification);
            Grid.SetColumn(_notification, 0);
            Grid.SetRow(_notification, 0);
            Grid.SetColumnSpan(_notification, 10);
            Grid.SetRowSpan(_notification, 10);
        }

        private void setNotification()
        {
            _notification = new TextBlock();
            _notification.Text = _shulteTestModel.GetDescription();
            _notification.Background = new SolidColorBrush(Colors.White);
            _notification.TextWrapping = TextWrapping.Wrap;
            _notification.FontSize = 24;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isTestDone)
            {
                MessageBoxResult mbResult = MessageBox.Show("Вы уверены, что хотите прервать прохождение теста? Результаты не будут сохранены", "Внимание", MessageBoxButton.OKCancel);
                if (mbResult != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

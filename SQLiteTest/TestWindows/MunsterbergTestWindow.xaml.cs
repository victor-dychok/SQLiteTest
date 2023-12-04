using SQLiteTest.Data;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
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
    /// Логика взаимодействия для MunsterbergTestWindow.xaml
    /// </summary>
    public partial class MunsterbergTestWindow : Window
    {
        private MunsterbergModel _munsterbergTest;
        private bool isCliced = false;
        private char[] _gridSymbols = null;
        private int _collumnNumber;
        private int _rowNumber;
        private int _currentCollumn;
        private int _currentRow;
        private int currentRightBorderX;
        private int currentLeftBorderX;
        private DateTime _time;
        private DatabaseContext _context;
        private bool _isTestDone = true;
        private User _user = null;

        public string GetInstruction()
        {
            return "Вам будет представлен массив букв, внутри которого спрятаны слова.\n" +
                "Вам необходимо найти все эти слова. Для того, чтобы выделить найденное слово зажмите левую кнопку мыши" +
                "и проведите курсор слева на право по этому слову. Для отмены выделения нужно выполнить то же действие, но в обратную сторону. " +
                "Так же вы можете выделить или отменить выделение на одном символе отдельно просто кликнув по нему.";
        }
        public MunsterbergTestWindow(DatabaseContext context)
        {
            InitializeComponent();
            numberOfWordsBox.SelectedIndex = 0;
            numberOfSymbolsBox.SelectedIndex = 0;
            Loaded += MunsterbergTestWindow_Loaded;
            _context = context;
        }

        private void MunsterbergTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _isTestDone = false;
        }

        public void SetUser(User user)
        {
            _user = user;
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            var numberOfWords = numberOfWordsBox.SelectedItem as TextBlock;
            var numberOfSymbols = numberOfSymbolsBox.SelectedItem as TextBlock;

            if (numberOfWords != null && numberOfSymbols != null)
            {
                generateGridItems(Convert.ToInt32(numberOfWords.Text), Convert.ToInt32(numberOfSymbols.Text));
                numberOfSymbolsBox.Visibility = Visibility.Collapsed;
                numberOfWordsBox.Visibility = Visibility.Collapsed;
                mainButton.Content = "Готово";
                _time = DateTime.UtcNow;
                mainButton.Click -= generateButton_Click;
                mainButton.Click += doneButton_Click;
            }
            else
            {
                MessageBox.Show("а это вообще как получилось?");
            }
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            var testResult = new MunsterbergTestResults(
                    getUserAnswer(),
                    _munsterbergTest.GetTestAnswer(),
                    Convert.ToUInt32((DateTime.UtcNow - _time).TotalSeconds),
                    _context);

            TestResultsWindow testResults = new TestResultsWindow(testResult, _user, testResult.GetChartValuesInterface());
            testResults.Show();
            _isTestDone = true;
            this.Close();
        }


        private void generateGridItems(int numberOfWords, int numberOfSymbols)
        {
            _munsterbergTest = new MunsterbergModel(numberOfWords, numberOfSymbols, new MunsterbergWordsProxy(_context));

            _collumnNumber = _munsterbergTest.GetCollumsNumber();
            _rowNumber = _munsterbergTest.GetRowNumber();
            _gridSymbols = _munsterbergTest.GetSymbolsArray();

            for (int i = 0; i < _collumnNumber; ++i)
            {
                symbolsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < _rowNumber; ++i)
            {
                symbolsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < _rowNumber; ++i)
            {
                for (int j = 0; j < _collumnNumber; ++j)
                {
                    var item = new TextBlock();
                    item.FontSize = 18;
                    item.TextAlignment = TextAlignment.Center;
                    item.Background = new SolidColorBrush(Colors.White);
                    if (j + i * _collumnNumber < _gridSymbols.Length)
                        item.Text = _gridSymbols[j + i * _collumnNumber].ToString();
                    else item.Text = "";
                    symbolsGrid.Children.Add(item);
                    Grid.SetColumn(item, j);
                    Grid.SetRow(item, i);
                }
            }
        }

        private List<string> getUserAnswer()
        {
            var answer = new List<string>();

            string currentString = "";
            bool isOnWord = false;
            bool isAdded = false;
            TextBlock block;
            SolidColorBrush bcColor;

            int i = 0;
            foreach (UIElement element in this.symbolsGrid.Children)
            {
                block = element as TextBlock;
                bcColor = block.Background as SolidColorBrush;
                if (block != null)
                {
                    if (bcColor.Color == Colors.LightGreen)
                    {
                        if (!isOnWord)
                        {
                            currentString = "";
                        }
                        currentString += block.Text;
                        isAdded = false;
                        isOnWord = true;
                    }
                    else if (!isAdded && currentString != "")
                    {
                        answer.Add(currentString);
                        isAdded = true;
                        isOnWord = false;
                    }
                }
                i++;
            }

            return answer;
        }

        private void symbolsGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isCliced = true;
            var point = Mouse.GetPosition(symbolsGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            foreach (var rowDefinition in symbolsGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            foreach (var columnDefinition in symbolsGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
            currentRightBorderX = currentLeftBorderX = (int)point.X;

            _currentCollumn = col;
            _currentRow = row;
            var element = GetElementInGridPosition(_currentCollumn, _currentRow) as TextBlock;
            var bcColor = element.Background as SolidColorBrush;
            if (bcColor.Color == Colors.LightGreen)
            {
                element.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                element.Background = new SolidColorBrush(Colors.LightGreen);
            }

        }

        private void symbolsGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCliced)
            {
                if (_currentCollumn < _collumnNumber)
                {
                    var point = Mouse.GetPosition(symbolsGrid);
                    var x = symbolsGrid.ColumnDefinitions[_currentCollumn];
                    SolidColorBrush bcgrBrush = new SolidColorBrush(Colors.White);

                    var element = GetElementInGridPosition(_currentCollumn, _currentRow) as TextBlock;
                    if (point.X > currentRightBorderX)
                    {
                        bcgrBrush = new SolidColorBrush(Colors.LightGreen);
                        currentLeftBorderX = currentRightBorderX;
                        currentRightBorderX += (int)x.ActualWidth;
                        _currentCollumn++;
                    }
                    else if (point.X < currentLeftBorderX)
                    {
                        currentRightBorderX = currentLeftBorderX;
                        currentLeftBorderX -= (int)x.ActualWidth;
                        if (_currentCollumn > 0)
                        {
                            _currentCollumn--;
                        }
                    }
                    element.Background = bcgrBrush;
                }
            }
        }

        private void symbolsGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isCliced = false;
        }

        private UIElement GetElementInGridPosition(int column, int row)
        {
            foreach (UIElement element in this.symbolsGrid.Children)
            {
                if (Grid.GetColumn(element) == column && Grid.GetRow(element) == row)
                    return element;
            }

            return null;
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
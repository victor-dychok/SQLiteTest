using SQLiteTest.Data;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Data.Repository;
using SQLiteTest.Models;
using SQLiteTest.TestWindows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLiteTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseContext context;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            context = new DatabaseContext();
            bool isCreatingDB = context.Database.EnsureCreated();
            if(isCreatingDB)
            {
                MessageBox.Show("База данных отсутствовала и была автоматически создана в процессе запуска программы.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void ShulteTestButton_Click(object sender, RoutedEventArgs e)
        {
            //var instructionWindow = new InstructionWindow(new ShulteTestWindow(context));
            //instructionWindow.Show();
            var reg = new RegistrationWindow(new ShulteTestWindow(context));
            reg.Show();
        }

        private void StrupTestButton_Click(object sender, RoutedEventArgs e)
        {
            //var instructionWindow = new InstructionWindow(new StrupTestWindow(StrupTestWindow.StrupTestModes.ModeWords, context));
            //instructionWindow.Show();
            var reg = new RegistrationWindow(new StrupTestWindow(StrupTestWindow.StrupTestModes.ModeWords, context));
            reg.Show();
        }

        private void munsterbergTestButton_Click(object sender, RoutedEventArgs e)
        {
            //var instructionWindow = new InstructionWindow(new MunsterbergTestWindow(context));
            //instructionWindow.Show();
            var reg = new RegistrationWindow(new MunsterbergTestWindow(context));
            reg.Show();
        }

        private void StrupTestButtonModeColors_Click(object sender, RoutedEventArgs e)
        {
            //var instructionWindow = new InstructionWindow(new StrupTestWindow(StrupTestWindow.StrupTestModes.ModeColors, context));
            //instructionWindow.Show();
            var reg = new RegistrationWindow(new StrupTestWindow(StrupTestWindow.StrupTestModes.ModeColors, context));
            reg.Show();
        }

        private void ResultsButton_Click(object sender, RoutedEventArgs e)
        {
            var resultWindow = new AllResultsWindow();
            resultWindow.Show();
        }
    }
}

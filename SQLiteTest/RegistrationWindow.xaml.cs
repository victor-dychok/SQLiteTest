using SQLiteTest.Data.Interfaces;
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
using System.Windows.Shapes;

namespace SQLiteTest
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        User _user;
        MunsterbergTestWindow _MunsterbergTestWindow;
        ShulteTestWindow _ShulteTestWindow;
        StrupTestWindow _StrupTestWindow;
        SolidColorBrush _incorrectInputBrush = new SolidColorBrush(Colors.Pink);
        SolidColorBrush _correctInputBrush = new SolidColorBrush(Colors.LightGreen);

        private enum Mode
        {
            Shulte,
            Strup,
            Munsterberg
        }

        private Mode _mode;

        public RegistrationWindow(MunsterbergTestWindow window)
        {
            InitializeComponent();
            _MunsterbergTestWindow = window;
            _mode = Mode.Munsterberg;
        }
        
        public RegistrationWindow(ShulteTestWindow window)
        {
            InitializeComponent();
            _ShulteTestWindow = window;
            _mode = Mode.Shulte;
        }
        
        public RegistrationWindow(StrupTestWindow window)
        {
            InitializeComponent();
            _StrupTestWindow = window;
            _mode = Mode.Strup;
        }

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            bool nameChecked = checkName();
            bool groupeChecked = checkGroupe();
            if (nameChecked && groupeChecked)
            {
                SetUser();
                Window instructionWindow = new Window();
                switch(_mode)
                {
                    case Mode.Shulte:
                        instructionWindow = new InstructionWindow(_ShulteTestWindow); break; 
                    case Mode.Strup:
                        instructionWindow = new InstructionWindow(_StrupTestWindow); break; 
                    case Mode.Munsterberg:
                        instructionWindow = new InstructionWindow(_MunsterbergTestWindow); break;
                }
                instructionWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Введены некоректные данные");
            }
        }

        private void noRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Window instructionWindow = new Window();
            switch (_mode)
            {
                case Mode.Shulte:
                    instructionWindow = new InstructionWindow(_ShulteTestWindow); break;
                case Mode.Strup:
                    instructionWindow = new InstructionWindow(_StrupTestWindow); break;
                case Mode.Munsterberg:
                    instructionWindow = new InstructionWindow(_MunsterbergTestWindow); break;
            }
            instructionWindow.Show();
            Close();
        }

        private bool checkName()
        {
            bool result = nameBox.Text != "";
            nameBox.Background = result ? _correctInputBrush : _incorrectInputBrush;
            return result;
        }
        private bool checkGroupe()
        {
            bool result = groupBox.Text != "";
            groupBox.Background = result ? _correctInputBrush : _incorrectInputBrush;
            return result;
        }

        private void SetUser()
        {
            User user = new User
            {
                Name = nameBox.Text,
                Group = groupBox.Text
            };

            switch (_mode)
            {
                case Mode.Shulte:
                    _ShulteTestWindow.SetUser(user); break;
                case Mode.Strup:
                    _StrupTestWindow.SetUser(user); break;
                case Mode.Munsterberg:
                    _MunsterbergTestWindow.SetUser(user); break;
            }
        }

    }
}

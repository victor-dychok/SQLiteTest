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
    /// Логика взаимодействия для InstructionWindow.xaml
    /// </summary>
    public partial class InstructionWindow : Window
    {
        private Window _windowToOpen;
        bool IsContinuing = false;
        public InstructionWindow(StrupTestWindow strupTest)
        {
            StartWindow(strupTest.GetInstruction());
            _windowToOpen = strupTest;
        }
        public InstructionWindow(ShulteTestWindow shulteTest)
        {
            StartWindow(shulteTest.GetInstruction());
            _windowToOpen = shulteTest;
        }
        public InstructionWindow(MunsterbergTestWindow munsterbergTest)
        {
            StartWindow(munsterbergTest.GetInstruction());
            _windowToOpen = munsterbergTest;
        }

        private void StartWindow(string info)
        {
            InitializeComponent();
            info += "\nДля того, чтобы продолжить нажмите кнопку \"Я ознакомился с инструкцией\"\nУспехов в выполнении!";
            InfoTextBox.Text = info;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            IsContinuing = true;
            _windowToOpen.Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!IsContinuing)
            {
                _windowToOpen.Close();
            }
        }
    }
}

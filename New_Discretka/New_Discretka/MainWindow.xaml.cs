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
using Microsoft.Win32;


namespace Labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InputClass inputClass;
        bool open,weigh;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception e)
            {

            }
        }

        private void input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";
            openFileDialog1.FileName = "deafault.txt";
            if (true == openFileDialog1.ShowDialog())
            {
                inputClass = new InputClass(openFileDialog1.FileName, inputBox, outputList,comboBoxIn,FirstBox,SecondBox);

                if (weigh)
                    inputClass.Output(true);
                else
                    inputClass.Output();

                inputClass.OutputMassive(true);

                open = true;
                

            }
        }

        private void buttonBFS_Click(object sender, RoutedEventArgs e)
        {
            inputClass.CreateVWidth(comboBoxIn.SelectedIndex);
        }

        private void buttonBFS_Copy_Click(object sender, RoutedEventArgs e)
        {
            inputClass.CreateVHeight(comboBoxIn.SelectedIndex);
            inputClass.Research();
        }

        private void buttonSort_Click(object sender, RoutedEventArgs e)
        {
            inputClass.ToSort();
        }

        private void buttonComponents_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Research();
        }

        private void button_Deck2_Click(object sender, RoutedEventArgs e)
        {
            //inputClass.twoDeckster(FirstBox.SelectedIndex, SecondBox.SelectedIndex,false);
            //inputClass.Ford(FirstBox.SelectedIndex, SecondBox.SelectedIndex, false);
            inputClass.Floyda(FirstBox.SelectedIndex, SecondBox.SelectedIndex, false);
            //inputClass.Floyda_Worshelaa(FirstBox.SelectedIndex, SecondBox.SelectedIndex);
        }

        private void button_DeckAll_Click(object sender, RoutedEventArgs e)
        {
            //inputClass.twoDeckster(FirstBox.SelectedIndex, SecondBox.SelectedIndex, true);
            //inputClass.Ford(FirstBox.SelectedIndex, SecondBox.SelectedIndex, true);
          
        }

        private void button__Floyd_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Floyda_Worshelaa();
        }

        private void button__EylirCircle_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Eylir_Circle();
            
        }

        private void button__Johnson_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Johnson();
        }

        private void button__Johnson_Co_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Johnson(FirstBox.SelectedIndex, SecondBox.SelectedIndex, false);
        }

        private void button_Gamilton_Click(object sender, RoutedEventArgs e)
        {
            inputClass.Gamilton();
        }

        private void weightBox_Checked(object sender, RoutedEventArgs e)
        {
            weigh = true;
        }
    }
}

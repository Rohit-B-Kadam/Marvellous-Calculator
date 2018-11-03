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
using System.Reflection;

namespace Marvellous_Calculator
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator cal;
      
        public MainWindow()
        {
            InitializeComponent();
            cal = new Calculator();
            DisplayOnScreen(cal.GetDisplayData());
            rbDec.IsChecked = true;
            btnCloseBkt.IsEnabled = false;
        }

        // Add data(binding) with combo box
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo.Name.Equals("BitComboBox"))
            {
                List<int> no = new List<int>();
                for (int i = 0; i <= 62; i++)
                    no.Add(i);

                combo.ItemsSource = no;
                combo.SelectedIndex = 0;
            }
            else
            {

                combo.ItemsSource = cal.NumberFunctName;
                combo.SelectedIndex = 0;

            }
        }

        public void DisplayOnScreen(string[] data)
        {
            if (data[0].Length <= 32)
            {
                txtScreen.FontSize = 35;
            }
            else
            {
                txtScreen.FontSize = 20;
            }

            txtScreen.Text = data[0];

            // display expression
            txtUppScreen.Text = data[1];
        }
        
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];
            switch ((sender as Button).Name)
            {
                case "btn0":
                    data = cal.DigitClick("0");
                    break;
                case "btn1":
                    data = cal.DigitClick("1");
                    break;
                case "btn2":
                    data = cal.DigitClick("2");
                    break;
                case "btn3":
                    data = cal.DigitClick("3");
                    break;
                case "btn4":
                    data = cal.DigitClick("4");
                    break;
                case "btn5":
                    data = cal.DigitClick("5");
                    break;
                case "btn6":
                    data = cal.DigitClick("6");
                    break;
                case "btn7":
                    data = cal.DigitClick("7");
                    break;
                case "btn8":
                    data = cal.DigitClick("8");
                    break;
                case "btn9":
                    data = cal.DigitClick("9");
                    break;
                case "btnA":
                    data = cal.DigitClick("A");
                    break;
                case "btnB":
                    data = cal.DigitClick("B");
                    break;
                case "btnC":
                    data = cal.DigitClick("C");
                    break;
                case "btnD":
                    data = cal.DigitClick("D");
                    break;
                case "btnE":
                    data = cal.DigitClick("E");
                    break;
                case "btnF":
                    data = cal.DigitClick("F");
                    break;
                case "btnPlusMinus":
                    data = cal.PlusMinusFunc();
                    break;
            }
            
            DisplayOnScreen(data);
        }

        private void Bracket_Click(object sender, RoutedEventArgs e)
        {

            string[] data = new string[2];
            switch ((sender as Button).Name)
            {
                // Brackets
                case "btnOpenBkt":
                    data = cal.OpenBracket();
                    break;
                case "btnCloseBkt":
                    data = cal.CloseBracket();
                    break;
            }

            DisplayOnScreen(data);
            if (cal.openBracketsCnt != 0)
                btnCloseBkt.IsEnabled = true;
            else
                btnCloseBkt.IsEnabled = false;
        }
        
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];
            switch ((sender as Button).Name)
            {
                // Arithmetric Operation
                case "btnAdd":
                    data = cal.BinaryOperator("+");
                    break;
                case "btnSubt":
                    data = cal.BinaryOperator("-");
                    break;
                case "btnMulti":
                    data = cal.BinaryOperator("*");
                    break;
                case "btnDiv":
                    data = cal.BinaryOperator("/");
                    break;
                case "btnMod":
                    data = cal.BinaryOperator("%");
                    break;

                // Binary Operation
                case "btnRsh":
                    data = cal.BinaryOperator(">>");
                    break;
                case "btnLsh":
                    data = cal.BinaryOperator("<<");
                    break;
                case "btnXor":
                    data = cal.BinaryOperator("^");
                    break;
                case "btnOr":
                    data = cal.BinaryOperator("|");
                    break;
                case "btnAnd":
                    data = cal.BinaryOperator("&");
                    break;
                case "btnNot":
                    data = cal.UniaryOperator("!");
                    break;

                // Equal
                case "btnEqual":
                    data = cal.EqualOpeartor();
                    break;

               // Bit Operation
                case "btnOffBit":
                    data = cal.BitwiseOperation("OffBit", BitComboBox.SelectedIndex);
                    break;
                case "btnOnBit":
                    data = cal.BitwiseOperation("OnBit", BitComboBox.SelectedIndex);
                    break;
                case "btnToggle":
                    data = cal.BitwiseOperation("ToggleBit", BitComboBox.SelectedIndex);
                    break;
            }
            
            DisplayOnScreen(data);
        }

        private void ClearOperation_Click(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];

            switch ((sender as Button).Name)
            {
                case "btnBackspace":
                    data = cal.GetDisplayData();
                    string buffer = data[0];
                    if (buffer.Length != 1)
                        buffer = buffer.Remove(buffer.Length - 1);
                    else
                        buffer = "0";

                    cal.value = cal.ConvertDataToDec(buffer);
                    data = cal.GetDisplayData();
                    data[1] += cal.value;
                    

                    break;
                case "btnClearAll":
                    cal.ClearAll();
                    data = cal.GetDisplayData();
                    break;
                case "btnClearE":
                    cal.value = "0";
                    data = cal.GetDisplayData();
                    data[1] += cal.value;
                    break;
            }
            DisplayOnScreen(data);
        }
        
        /// <summary>
        ///  This function is to convert the data into select format.
        ///  And Disable and enable the button respective to select the type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conversion_Click(object sender, RoutedEventArgs e)
        {
            
            string cn = (sender as RadioButton).Name;
            switch ( cn )
            {
                case "rbHex":
                    //cal.screenData = cal.DataConversion(cal.screenData, cal.displayType, "Hex");
                    cal.displayType = "Hex";
                    break;
                case "rbDec":
                    //cal.screenData = cal.DataConversion(cal.screenData, cal.displayType, "Dec");
                    cal.displayType = "Dec";
                    break;
                case "rbOct":
                    //cal.screenData = cal.DataConversion(cal.screenData, cal.displayType, "Oct");
                    cal.displayType = "Oct";
                    break;
                case "rbBin":
                    //cal.screenData = cal.DataConversion(cal.screenData, cal.displayType, "Bin");
                    cal.displayType = "Bin";
                    break;
            }

            if( cn.Equals("rbDec") || cn.Equals("rbOct") || cn.Equals("rbBin"))
            {
                btnA.IsEnabled = false;
                btnB.IsEnabled = false;
                btnC.IsEnabled = false;
                btnD.IsEnabled = false;
                btnE.IsEnabled = false;
                btnF.IsEnabled = false;
            }
            else
            {
                btnA.IsEnabled = true;
                btnB.IsEnabled = true;
                btnC.IsEnabled = true;
                btnD.IsEnabled = true;
                btnE.IsEnabled = true;
                btnF.IsEnabled = true;
            }

            if(cn.Equals("rbOct") || cn.Equals("rbBin"))
            {
                btn9.IsEnabled = false;
                btn8.IsEnabled = false;
            }
            else
            {
                btn9.IsEnabled = true;
                btn8.IsEnabled = true;
            }

            if (cn.Equals("rbBin"))
            {
                btn7.IsEnabled = false;
                btn6.IsEnabled = false;
                btn5.IsEnabled = false;
                btn4.IsEnabled = false;
                btn3.IsEnabled = false;
                btn2.IsEnabled = false;
            }
            else
            {
                btn7.IsEnabled = true;
                btn6.IsEnabled = true;
                btn5.IsEnabled = true;
                btn4.IsEnabled = true;
                btn3.IsEnabled = true;
                btn2.IsEnabled = true;
            }

            if(cn.Equals("rbHex") || cn.Equals("rbOct") || cn.Equals("rbBin"))
            {
                btnPlusMinus.IsEnabled = false;
            }
            else
            {
                if (!cal.value.Equals("0"))
                {
                    btnPlusMinus.IsEnabled = true;
                }
            }

            DisplayOnScreen(cal.GetDisplayData());
            txtUppScreen.Text += cal.value;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            string name = selectedComboItem.SelectedItem as string;
            if( name.Contains("Is"))
            {
                btnCheck.Content = "Check";
            }
            else
            {
                btnCheck.Content = "Get";
            }
           
        }
        
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            cal.NumberFunction(FunctionComboBox.SelectedItem.ToString());
        }

        // Close buttom event
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            string message = "Do you want to save the history";
            string caption = "Save";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            // Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);

            // if user click no
            if (result.HasFlag(MessageBoxResult.No))
            {
                cal.SaveFileOrNot('n');
            }
            else
            {
                cal.SaveFileOrNot('y');
            }
        }
    }
}

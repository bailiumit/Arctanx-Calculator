using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;                        //需要用到线程
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
using System.Text.RegularExpressions;


namespace Arctan
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            Input_TextBox.Text = "1";
            Accuracy_TextBox.Text = "20";
            Taylor_TextBox.Text = "计算结果";
            TaylorTime_TextBox.Text = "耗时";
            Romberg_TextBox.Text = "计算结果";
            RombergTime_TextBox.Text = "耗时";
            Newton_TextBox.Text = "计算结果";
            NewtonTime_TextBox.Text = "耗时";
        }

        private void Button_Click_Calc(object sender, RoutedEventArgs e)
        {
            string inputNum_string = Input_TextBox.Text;
            Number inputNum = new Number(inputNum_string, false, 0, 0);
            string inputAccuracy_string = Accuracy_TextBox.Text;
            int accuracy = Convert.ToInt32(inputAccuracy_string);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            //进行计算
            RangeAssist rangeAssist = new RangeAssist(inputNum, accuracy);
            //泰勒展开法进行计算
            stopwatch.Start();
            Taylor TaylorCal = new Taylor(rangeAssist.numConverted, accuracy);
            Number resultTaylor = rangeAssist.NumRecover(TaylorCal.TaylorCalculate());
            stopwatch.Stop();
            float taylorTime = (float)stopwatch.ElapsedMilliseconds / 1000;
            //外推加速法进行计算
            stopwatch.Start();
            Romberg RombergCal = new Romberg(rangeAssist.numConverted, accuracy);
            Number resultRomberg = rangeAssist.NumRecover(RombergCal.RombergCalculate());
            stopwatch.Stop();
            float rombergTime = (float)stopwatch.ElapsedMilliseconds / 1000;
            //牛顿法进行计算
            stopwatch.Start();
            Newton NewtonCal = new Newton(rangeAssist.numConverted, accuracy);
            Number resultNewton = rangeAssist.NumRecover(NewtonCal.NewtonCalculate());
            stopwatch.Stop();
            float newtonTime = (float)stopwatch.ElapsedMilliseconds / 1000;

            //显示字符串
            //泰勒法
            string resultTaylor_string = "";
            if (resultTaylor.sign == -1)
            {
                resultTaylor_string += "-";
            }
            for (int i = 0; i < resultTaylor.intLength; i++)
            {
                resultTaylor_string += resultTaylor.intPart[i].ToString();
            }
            resultTaylor_string += ".";
            for (int i = 0; i < resultTaylor.decLength; i++)
            {
                resultTaylor_string += resultTaylor.decPart[i].ToString();
            }
            Taylor_TextBox.Text = resultTaylor_string;
            TaylorTime_TextBox.Text = taylorTime.ToString() + "秒";
            //外推加速法
            string resultRomberg_string = "";
            if (resultRomberg.sign == -1)
            {
                resultRomberg_string += "-";
            }
            for (int i = 0; i < resultRomberg.intLength; i++)
            {
                resultRomberg_string += resultRomberg.intPart[i].ToString();
            }
            resultRomberg_string += ".";
            for (int i = 0; i < resultRomberg.decLength; i++)
            {
                resultRomberg_string += resultRomberg.decPart[i].ToString();
            }
            Romberg_TextBox.Text = resultRomberg_string;
            RombergTime_TextBox.Text = rombergTime.ToString() + "秒";
            //牛顿法
            string resultNewton_string = "";
            if (resultNewton.sign == -1)
            {
                resultNewton_string += "-";
            }
            for (int i = 0; i < resultNewton.intLength; i++)
            {
                resultNewton_string += resultNewton.intPart[i].ToString();
            }
            resultNewton_string += ".";
            for (int i = 0; i < resultNewton.decLength; i++)
            {
                resultNewton_string += resultNewton.decPart[i].ToString();
            }
            Newton_TextBox.Text = resultNewton_string;
            NewtonTime_TextBox.Text = newtonTime.ToString() + "秒";
        }

        private void Input_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Accuracy_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}

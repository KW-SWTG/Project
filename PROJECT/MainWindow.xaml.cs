using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace PROJECT
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private int cnt = 0;

        public MainWindow()
        {
            InitializeComponent();


            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void poster1_Click(object sender, RoutedEventArgs e)
        {
            cnt++;
            if (cnt == 5)
            {
                Page1 p1 = new Page1();
                this.Content = p1;
            }
            
                //사진바꾸기..!
        }

        private void poster2_Click(object sender, RoutedEventArgs e)
        {
            cnt++;
            if (cnt == 5)
            {
                Page1 p1 = new Page1();
                this.Content = p1;
            }

            //사진바꾸기..!
        }

        private void poster3_Click(object sender, RoutedEventArgs e)
        {
            cnt++;
            if (cnt == 5)
            {
                Page1 p1 = new Page1();
                this.Content = p1;
            }

            //사진바꾸기..!
        }

        private void poster4_Click(object sender, RoutedEventArgs e)
        {
            cnt++;
            if (cnt == 5)
            {
                Page1 p1 = new Page1();
                this.Content = p1;
            }

            //사진바꾸기..!
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

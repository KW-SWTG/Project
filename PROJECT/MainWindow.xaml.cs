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
        List<string> movieList;
        private int cnt = 0;

        public MainWindow()
        {
            InitializeComponent();

            movieList = new List<string>();

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void posterOption_Click(object sender, RoutedEventArgs e)
        {
            var posterOption = sender as Button;

            if(null != posterOption)
            {
                cnt++;

                if(cnt == 5)
                {
                    // 취향분석 알고리즘을 통해 얻은 추천영화 리스트 
                    string[] title = { "라라랜드", "코코", "어벤져스", "택시운전사", "1917", "신세계" };      // 임시

                    List<string> list = new List<string>();
                    list.AddRange(title);

                    Page1 p1 = new Page1(list);
                    this.Content = p1;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

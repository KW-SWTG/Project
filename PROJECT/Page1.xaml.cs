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
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {
        List<string> movieList;
        List<MovieInfo> movieInfos;

        public Page1(List<string> list)
        {
            InitializeComponent();
            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList"];

            Init(list);
        }

        private void Init(List<string> list)
        {
            // get 추천영화 리스트 10개(제목들, 카테고리) from MainWindow.xaml
            movieList = new List<string>(list);

            // debug
            btnMv1.Content = list[0];
            btnMv2.Content = list[1];
            btnMv3.Content = list[2];
            btnMv4.Content = list[3];
            btnMv5.Content = list[4];
            btnMv6.Content = list[5];
            btnMv7.Content = list[6];
            btnMv8.Content = list[7];
            btnMv9.Content = list[8];
            btnMv10.Content = list[9];

        }

        private void btnMv_Click(object sender, RoutedEventArgs e)
        {
            var btnOption = sender as Button;

            if (null != btnOption)
            {
                string title = "";

                // 영화 제목
                if (btnOption == btnMv1)
                    title = btnMv1.Content.ToString();
                else if (btnOption == btnMv2)
                    title = btnMv2.Content.ToString();
                else if (btnOption == btnMv3)
                    title = btnMv3.Content.ToString();
                else if (btnOption == btnMv4)
                    title = btnMv4.Content.ToString();
                else if (btnOption == btnMv5)
                    title = btnMv5.Content.ToString();
                else if (btnOption == btnMv6)
                    title = btnMv6.Content.ToString();
                else if (btnOption == btnMv7)
                    title = btnMv7.Content.ToString();
                else if (btnOption == btnMv8)
                    title = btnMv8.Content.ToString();
                else if (btnOption == btnMv9)
                    title = btnMv9.Content.ToString();
                else if (btnOption == btnMv10)
                    title = btnMv10.Content.ToString();

                // new Window and send extraData to Window1.xaml
                Window1 w1 = new Window1(title);
                w1.Show();
            }
        }

        private void btnKill_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
            App.Current.MainWindow.Close();
            Environment.Exit(0);
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}

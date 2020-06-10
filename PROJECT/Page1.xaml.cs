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
            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList2"];

            Init(list);
        }

        private void Init(List<string> list)
        {
            // get 추천영화 리스트 10개(제목들, 카테고리) from MainWindow.xaml
            movieList = new List<string>(list);
            printMovies(list);
        }

        private void printMovies(List<string> list)
        {
            List<ImageBrush> brushes = new List<ImageBrush>();

            for (int i = 0; i < list.Count; i++)
            {
                string posterUrl = JsonLib.findPosterUrl(list[i], movieInfos);
                if (posterUrl.Equals(""))
                {
                    brushes.Add(null);
                    continue;
                }

                BitmapImage bitmapImg = new BitmapImage(new Uri(posterUrl, UriKind.RelativeOrAbsolute));
                ImageBrush brush = new ImageBrush();

                brush.ImageSource = bitmapImg;
                brushes.Add(brush);
            }

            btnMv1.Background = brushes[0];
            btnMv2.Background = brushes[1];
            btnMv3.Background = brushes[2];
            btnMv4.Background = brushes[3];
            btnMv5.Background = brushes[4];
            btnMv6.Background = brushes[5];
            btnMv7.Background = brushes[6];
            btnMv8.Background = brushes[7];
            btnMv9.Background = brushes[8];
            btnMv10.Background = brushes[9];
        }

        private void btnMv_Click(object sender, RoutedEventArgs e)
        {
            var btnOption = sender as Button;

            if (null != btnOption)
            {
                string title = "";

                // 영화 제목
                if (btnOption == btnMv1)
                    title = movieList[0];
                else if (btnOption == btnMv2)
                    title = movieList[1];
                else if (btnOption == btnMv3)
                    title = movieList[2];
                else if (btnOption == btnMv4)
                    title = movieList[3];
                else if (btnOption == btnMv5)
                    title = movieList[4];
                else if (btnOption == btnMv6)
                    title = movieList[5];
                else if (btnOption == btnMv7)
                    title = movieList[6];
                else if (btnOption == btnMv8)
                    title = movieList[7];
                else if (btnOption == btnMv9)
                    title = movieList[8];
                else if (btnOption == btnMv10)
                    title = movieList[9];

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

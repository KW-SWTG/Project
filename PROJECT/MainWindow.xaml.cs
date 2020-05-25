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
        List<MovieInfo> movieInfos;
        private int cnt = 0;

        public MainWindow()
        {
            InitializeComponent();
            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList"];

            ClickedChange();

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void posterOption_Click(object sender, RoutedEventArgs e)
        {
            ClickedChange();
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
        private void ClickedChange()
        {
            var random = new Random();
            List<MovieInfo> rand10mov = new List<MovieInfo>();
            List<ImageBrush> brushes = new List<ImageBrush>();

            for (int k = 0; k < 10; k++)
            {
                rand10mov.Add(movieInfos[random.Next(movieInfos.Count)]);
                BitmapImage i = new BitmapImage(new Uri(rand10mov[k].MoviePoster, UriKind.RelativeOrAbsolute));
                ImageBrush brush = new ImageBrush();

                brush.ImageSource = i;
                brushes.Add(brush);
            }

            poster1.Background = brushes[0];
            poster2.Background = brushes[1];
            poster3.Background = brushes[2];
            poster4.Background = brushes[3];
            Poster5.Background = brushes[4];
            Poster6.Background = brushes[5];
            Poster7.Background = brushes[6];
            Poster8.Background = brushes[7];
            Poster9.Background = brushes[8];
            Poster10.Background = brushes[9];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

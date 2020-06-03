using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class Window1 : Window
    {
        MovieDB db;
        List<MovieInfo> movieInfos;

        public Window1(string extraData)
        {
            InitializeComponent();
            CenterWindowOnScreen();

            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList"];
            InitMovie(extraData);

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }

        private void CenterWindowOnScreen()
        {
            this.Left = Application.Current.MainWindow.Left;
            this.Top = Application.Current.MainWindow.Top;
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitMovie(string extraData)
        {
            db = new MovieDB();

            // get title from Page1.xaml
            db.title = extraData;
            db.url = JsonLib.findMovieUrl(db.title, movieInfos);
            db.posterUrl = JsonLib.findPosterUrl(db.title, movieInfos);

            // init name of label(=mvName)
            mvName.Content = db.title;

            // load movie
            Uri url = new Uri(db.url);
            webBrowser.Navigate(url.AbsoluteUri);
        }

        private string printList(List<string> tmp)
        {
            if (!tmp.Any())
                return null;

            string str = tmp[0];
            for (int i = 1; i < tmp.Count; i++)
                str += ", " + tmp[i];
            return str;
        }

        private void printMovie()
        {
            // 전문가 평점, 관객 평점, 상영 시간
            proRate.Text = db.expRating;
            audRate.Text = db.audRating;
            runningTimeTxt.Text = db.runningTime;

            // 감독, 장르, 관람 등급, 국가
            directorTxt.Text = printList(db.director);
            genreTxt.Text = printList(db.genre);
            mvRatingTxt.Text = db.movieRating;
            nationTxt.Text = printList(db.nation);
        }

        private void printRecommendMovies()
        {
            List<ImageBrush> brushes = new List<ImageBrush>();

            for (int i = 0; i < 5; i++)
            {
                string posterUrl = JsonLib.findPosterUrl(db.recommendMovies[i], movieInfos);
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

            rePoster1.Background = brushes[0];
            rePoster2.Background = brushes[1];
            rePoster3.Background = brushes[2];
            rePoster4.Background = brushes[3];
            rePoster5.Background = brushes[4];
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (WebLib.UpdateMovieDB(webBrowser.Document as mshtml.HTMLDocument, db))
            {
                mvImg.Stretch = Stretch.Uniform;
                mvImg.Source = new BitmapImage(new Uri(db.posterUrl));

                printMovie();
                printRecommendMovies();
            }
        }

        private void rePosterOption_Click(object sender, RoutedEventArgs e)
        {
            Button posterOption = sender as Button;
            int index = -1;

            if (posterOption.Name == rePoster1.Name)
                index = 0;
            else if (posterOption.Name == rePoster2.Name)
                index = 1;
            else if (posterOption.Name == rePoster3.Name)
                index = 2;
            else if (posterOption.Name == rePoster4.Name)
                index = 3;
            else if (posterOption.Name == rePoster5.Name)
                index = 4;

            if (index > -1)
            {
                string name = db.recommendMovies[index];

                this.Close();
                Window1 newWindow = new Window1(name);
                newWindow.Show();
            }
        }
    }
}

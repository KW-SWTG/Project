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
        
        public Window1(string extraData)
        {
            InitializeComponent();
            CenterWindowOnScreen();
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

            // get title&url from Page1.xaml
            string[] data = extraData.Split(',');
            db.title = data[0];
            db.url = data[1];

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

        private void loadPoster()
        {
            if (WebLib.findPoster(webBrowser.Document as mshtml.HTMLDocument, db))
            {
                mvImg.Stretch = Stretch.Uniform;
                mvImg.Source = new BitmapImage(new Uri(db.posterUrl));
            }
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (WebLib.UpdateMovieDB(webBrowser.Document as mshtml.HTMLDocument, db))
            {
                loadPoster();
                printMovie();
            }
        }
    }
}

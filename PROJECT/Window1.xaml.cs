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

        // debug
        List<MovieInfo> movieInfos;

        public Window1()
        {
            InitializeComponent();

            InitMovie();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitMovie()
        {
            db = new MovieDB();

            // MovieInfos - how to make them global..?
            movieInfos = new List<MovieInfo>();
            JsonLib.InitMovieInfo(movieInfos);

            // get title & url
            db.title = "쇼생크 탈출";
            db.url = JsonLib.findMovieUrl(db.title, movieInfos);
            if (db.url.Length <= 0)
                return;

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
            StringBuilder str = new StringBuilder();
            proRate.Text = db.expRating;
            audRate.Text = db.audRating;
            runTIme.Text = db.runningTime;
            //direct, enre, age counrey, rintom
            direct.Text =printList(db.director);
            Genre.Text = printList(db.genre);
            age.Text = db.movieRating;
            country.Text = printList(db.nation);


            

            //str.AppendLine("제목 : " + db.title);
            //str.AppendLine("관람객 평점 : " + db.audRating);
            //str.AppendLine("전문가 평점 : " + db.expRating);
           // str.AppendLine("네티즌 평점 : " + db.netRating);
           // str.AppendLine("개봉일 : " + db.releaseDate);
            //str.AppendLine("상영 시간 : " + db.runningTime);
            //str.AppendLine("관람 등급 : " + db.movieRating);
            //str.AppendLine("장르 : " + printList(db.genre));
            //str.AppendLine("국가 : " + printList(db.nation));
            //str.AppendLine("감독 : " + printList(db.director));
            //str.AppendLine("배우 : " + printList(db.actor));
            //str.AppendLine("유사한 영화들 : " + printList(db.recommendMovies));
            
            //txtMovie.Text = str.ToString();
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        List<MovieInfo> movieInfos = new List<MovieInfo>();
        List<MovieInfo> rand10mov = new List<MovieInfo>();
        List<MovieInfo> AlreadyShowlst = new List<MovieInfo>();
        List<MovieInfo> SelectedMovlst = new List<MovieInfo>();
        List<MovieInfo> RecommandMovieInfos = new List<MovieInfo>();
        List<Button> btnlist = new List<Button>();
        private int cnt = 0;

        public MainWindow()
        {
            InitializeComponent();
            btnlist.Add(poster1);
            btnlist.Add(poster2);
            btnlist.Add(poster3);
            btnlist.Add(poster4);
            btnlist.Add(Poster5);
            btnlist.Add(Poster6);
            btnlist.Add(Poster7);
            btnlist.Add(Poster8);
            btnlist.Add(Poster9);
            btnlist.Add(Poster10);
            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList"];
            RecommandMovieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList2"];
            ClickedChange();

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void posterOption_Click(object sender, RoutedEventArgs e)
        {

            var posterOption = sender as Button;
            if (posterOption.Name == "Move")
            {

                for (int i = 0; i < 10; i++)
                {
                    if (btnlist[i].Tag.ToString() == "checked")
                    {
                        SelectedMovlst.Add(rand10mov[i]);
                        cnt++;
                    }
                }
                if (cnt >= 30)
                {

                    List<string> Genre = RcmMovlst(SelectedMovlst);
                    List<string> Actor = ActorsCounts(SelectedMovlst);
                    List<string> Country = CountryCounts(SelectedMovlst);

                    // 취향분석 알고리즘을 통해 얻은 추천영화 리스트
                    List<string> Rcmlst = RecommandMovies(Genre, Actor, Country);
                    foreach (var item in Rcmlst)
                    {
                        Console.WriteLine(item);
                    }
                    List<string> list = new List<string>();


                    Page1 p1 = new Page1(Rcmlst);
                    this.Content = p1;
                }
            }
            ClickedChange();
        }
        private void PosterClick(object sender, RoutedEventArgs e)
        {

            var posterOption = sender as Button;
            if (posterOption.Tag.ToString() == "unchecked")
            {
                posterOption.Tag = "checked";
                posterOption.BorderThickness = new Thickness(10);
                posterOption.BorderBrush = Brushes.Red;
            }
            else
            {
                posterOption.Tag = "unchecked";
                posterOption.BorderThickness = new Thickness(1);
                posterOption.BorderBrush = Brushes.Black;
            }

        }

        private void ClickedChange()
        {

            initPoster();
            var random = new Random();
            foreach (var Mov in rand10mov)
            {
                AlreadyShowlst.Add(Mov);
            }
            rand10mov.Clear();
            List<ImageBrush> brushes = new List<ImageBrush>();

            for (int k = 0; k < 10; k++)
            {

                var tempmov = movieInfos[random.Next(movieInfos.Count)];
                while (tempmov.MoviePoster == "None" && AlreadyShowlst.Contains(tempmov) == false)
                {
                    tempmov = movieInfos[random.Next(movieInfos.Count)];
                }

                BitmapImage i = new BitmapImage(new Uri(tempmov.MoviePoster, UriKind.RelativeOrAbsolute));
                rand10mov.Add(tempmov);
                ImageBrush brush = new ImageBrush();

                brush.ImageSource = i;
                brushes.Add(brush);
            }


            for (int i = 0; i < 10; i++)
            {
                btnlist[i].Background = brushes[i];
            }
        }


        private void initPoster()
        {

            foreach (var item in btnlist)
            {
                item.Tag = "unchecked";
                item.BorderBrush = Brushes.Black;
                item.BorderThickness = new Thickness(1);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private List<string> RcmMovlst(List<MovieInfo> selectedlst)
        {
            List<string> Genrelst = new List<string>();
            Dictionary<string, double> movieDic = new Dictionary<string, double>();
            for (int i = 0; i < selectedlst.Count; i++)
            {
                int t = selectedlst[i].Genre.Count();
                for (int k = 0; k < t; k++)
                {
                    double grScore = 1 / (double)t;
                    if (t == 1 && selectedlst[i].Genre[k] == "드라마")
                    {
                        if (Genrelst.Contains(selectedlst[i].Genre[k]) == false)
                        {
                            Genrelst.Add(selectedlst[i].Genre[k]);
                            movieDic.Add(selectedlst[i].Genre[k], 0);
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                        else
                        {
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                    }
                    else if (selectedlst[i].Genre.Contains("드라마") == true)
                    {
                        grScore = 1 / (double)(t - 1);
                        if (selectedlst[i].Genre[k] == "드라마")
                            continue;
                        if (Genrelst.Contains(selectedlst[i].Genre[k]) == false)
                        {
                            Genrelst.Add(selectedlst[i].Genre[k]);
                            movieDic.Add(selectedlst[i].Genre[k], 0);
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                        else
                        {
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                    }
                    else
                    {
                        if (Genrelst.Contains(selectedlst[i].Genre[k]) == false)
                        {
                            Genrelst.Add(selectedlst[i].Genre[k]);
                            movieDic.Add(selectedlst[i].Genre[k], 0);
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                        else
                        {
                            movieDic[selectedlst[i].Genre[k]] += Math.Round(grScore, 3);
                        }
                    }
                }
            }
            //선택영화당 장르별 가중치 계산하여 구하고 dictionary에 저장

            var sortedmovieDic = movieDic.OrderByDescending(num => num.Value);

            double averageGenreSC = 0;
            List<double> gsArr = new List<double>();
            foreach (KeyValuePair<string, double> tt in sortedmovieDic)
            {
                Console.WriteLine("key:{0}, Value:{1}", tt.Key, tt.Value);
                averageGenreSC += tt.Value;
                gsArr.Add(tt.Value);
            }

            Console.WriteLine("AVG "+averageGenreSC / sortedmovieDic.Count());
            Console.WriteLine("STD " + StdDev(gsArr));


            //장르별로 표준편차 계산

            int casenum = 0;
            List<string> Fgenrelst = new List<string>();

            if (StdDev(gsArr) + averageGenreSC / sortedmovieDic.Count() < sortedmovieDic.ElementAt(0).Value)
                casenum = 1;
            else
                casenum = 2;

            switch (casenum)
            {
                case 1:
                    foreach (KeyValuePair<string, double> tt in sortedmovieDic)
                    {
                        if (tt.Value > StdDev(gsArr) + averageGenreSC / sortedmovieDic.Count())
                            Fgenrelst.Add(tt.Key);
                    }
                    break;
                case 2:
                    //foreach (KeyValuePair<string, double> tt in sortedmovieDic)
                    //{
                    //    if (tt.Value > averageGenreSC / sortedmovieDic.Count() - StdDev(gsArr))
                    //        Fgenrelst.Add(tt.Key);
                    //}
                    Fgenrelst.Add(sortedmovieDic.ElementAt(0).Key);
                    break;
                default:
                    Console.WriteLine("Wrong Genre Search");
                    break;
            }
            //표준편차구간의 최상의 장르의 유무에 따라 추천 장르 저장
            foreach (var item in Fgenrelst)
            {
                Console.WriteLine(item);
            }
            return Fgenrelst;
        }

        private List<string> ActorsCounts(List<MovieInfo> selectedlst)
        {
            List<string> Actorlst = new List<string>();
            Dictionary<string, int> movieDic = new Dictionary<string, int>();
            for (int i = 0; i < selectedlst.Count; i++)
            {
                int t = selectedlst[i].Actor.Count();
                for (int k = 0; k < t; k++)
                {
                    if (Actorlst.Contains(selectedlst[i].Actor[k]) == false)
                    {
                        Actorlst.Add(selectedlst[i].Actor[k]);
                        movieDic.Add(selectedlst[i].Actor[k], 0);
                        movieDic[selectedlst[i].Actor[k]] += 1;
                    }
                    else
                    {
                        movieDic[selectedlst[i].Actor[k]] += 1;
                    }
                }
            }

            var sortedmovieDic = movieDic.OrderByDescending(num => num.Value);
            List<string> RActorlst = new List<string>();
            foreach (KeyValuePair<string, int> tt in sortedmovieDic)
            {
                Console.WriteLine("key:{0}, Value:{1}", tt.Key, tt.Value);
                if (tt.Value > 8)
                    RActorlst.Add(tt.Key);
            }
            if (RActorlst.Count() < 1)
                RActorlst.Add("None");

            return RActorlst;
        }

        private List<string> CountryCounts(List<MovieInfo> selectedlst)
        {
            List<string> Countrylst = new List<string>();
            Dictionary<string, int> movieDic = new Dictionary<string, int>();
            for (int i = 0; i < selectedlst.Count; i++)
            {
                int t = selectedlst[i].Country.Count();
                for (int k = 0; k < t; k++)
                {
                    if (Countrylst.Contains(selectedlst[i].Country[k]) == false)
                    {
                        Countrylst.Add(selectedlst[i].Country[k]);
                        movieDic.Add(selectedlst[i].Country[k], 0);
                        movieDic[selectedlst[i].Country[k]] += 1;
                    }
                    else
                    {
                        movieDic[selectedlst[i].Country[k]] += 1;
                    }
                }
            }

            var sortedmovieDic = movieDic.OrderByDescending(num => num.Value);
            List<string> RCountrylst = new List<string>();
            foreach (KeyValuePair<string, int> tt in sortedmovieDic)
            {
                Console.WriteLine("key:{0}, Value:{1}", tt.Key, tt.Value);
                if (tt.Value > 15)
                    RCountrylst.Add(tt.Key);
            }
            if (RCountrylst.Count() < 1)
                RCountrylst.Add("None");
            return RCountrylst;
        }


        private List<string> RecommandMovies(List<string> Genrelst, List<string> Actorlst, List<string> Countrylst)
        {
            List<string> Recommandlst = new List<string>();
            List<string> UrlNum = new List<string>();
            int GenreCounts = Genrelst.Count();
            var tempmov = new MovieInfo();
            var random = new Random();
            int movCount = 0;            
            if (Actorlst[0] != "None")
            {
                while (Recommandlst.Count() != 10)
                {
                    tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                    if (tempmov.Genre != null && tempmov.Actor.Contains(Actorlst[0]) == true && Recommandlst.Contains(tempmov.MovieName) == false && UrlNum.Contains(tempmov.Url.Split('=')[1]) == false)
                    {
                        Console.WriteLine(tempmov.Url.Split('=')[1]);
                        Recommandlst.Add(tempmov.MovieName);
                        UrlNum.Add(tempmov.Url.Split('=')[1]);
                    }
                    movCount++;
                    if (movCount > 30000)
                    {
                        while (Recommandlst.Count() != 10)
                        {
                            tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                            if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                            {
                                Recommandlst.Add(tempmov.MovieName);
                            }
                        }
                        break;
                    }
                }
            }
            //Actor Selected
            else
            {
                Console.WriteLine(Countrylst[0]);
                if (Countrylst[0] != "None")
                {
                    switch (GenreCounts)
                    {
                        case 1:
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0]==Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        case 2:
                            while (Recommandlst.Count() != 5)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Contains(Genrelst[1]) && tempmov.Genre.Count() == 2 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[1]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        case 3:
                            while (Recommandlst.Count() != 4 || movCount < 30000)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Contains(Genrelst[1]) && tempmov.Genre.Contains(Genrelst[2]) && tempmov.Genre.Count() == 3 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                                movCount++;
                            }
                            while (Recommandlst.Count() != 7)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[1]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        default:
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && tempmov.Country[0] == Countrylst[0] && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                    }
                }
                //country selected
                else
                {
                    switch (GenreCounts)
                    {
                        case 1:
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        case 2:
                            while (Recommandlst.Count() != 4)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Contains(Genrelst[1]) && tempmov.Genre.Count() == 2 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 7)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[1]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        case 3:
                            while (Recommandlst.Count() != 4 || movCount < 30000)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Contains(Genrelst[1]) && tempmov.Genre.Contains(Genrelst[2]) && tempmov.Genre.Count() == 3 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                                movCount++;
                            }
                            while (Recommandlst.Count() != 7)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[1]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                        default:
                            while (Recommandlst.Count() != 10)
                            {
                                tempmov = RecommandMovieInfos[random.Next(RecommandMovieInfos.Count())];
                                if (tempmov.Genre != null && tempmov.Genre.Contains(Genrelst[0]) == true && tempmov.Genre.Count() == 1 && Recommandlst.Contains(tempmov.MovieName) == false)
                                {
                                    Recommandlst.Add(tempmov.MovieName);
                                }
                            }
                            break;
                    }
                }
            }
            return Recommandlst;
        }
        private double StdDev(IEnumerable<double> values)
        {
            double result = 0;
            try
            {
                if (values.Count() > 0)
                {
                    double mean = values.Average();
                    double sum = values.Sum(d => Math.Pow(d - mean, 2));
                    result = Math.Sqrt((sum) / (values.Count() - 1));
                }
            }
            catch { }
            return result;
        }
    }
}

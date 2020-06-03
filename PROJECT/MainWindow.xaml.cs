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
        List<MovieInfo> rand10mov = new List<MovieInfo>();
        List<MovieInfo> AlreadyShowlst = new List<MovieInfo>();
        List<MovieInfo> SelectedMovlst = new List<MovieInfo>();
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
            
            var posterOption = sender as Button;
            ClickedChange();

            if (null != posterOption && posterOption.Name !="Move")
            {
                cnt++;
                
                int btnnum =  Convert.ToInt32(posterOption.Name.ToString().Last().ToString());
                if (btnnum == 0)
                    btnnum = 10;
                SelectedMovlst.Add(rand10mov[btnnum - 1]);
                if (cnt == 30)
                {
                    
                    List<string> Genre = RcmMovlst(SelectedMovlst);
                    //List<string> Actor =
                    //List<string> Country =
                    // 취향분석 알고리즘을 통해 얻은 추천영화 리스트

                    List<string> list = new List<string>();
                    string[] Movielst = { "라라랜드", "코코", "어벤져스", "택시운전사", "1917", "신세계" };
                    list.AddRange(Movielst);

                    Page1 p1 = new Page1(list);
                    this.Content = p1;
                }
            }
        }
        private void ClickedChange()
        {
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
                while (tempmov.MoviePoster == "None" && AlreadyShowlst.Contains(tempmov)==true)
                {
                    tempmov = movieInfos[random.Next(movieInfos.Count)];
                }

                BitmapImage i = new BitmapImage(new Uri(tempmov.MoviePoster, UriKind.RelativeOrAbsolute));
                rand10mov.Add(tempmov);
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

        private List<string> RcmMovlst(List<MovieInfo> selectedlst)
        {
            List<string> Genrelst = new List<string>();
            Dictionary<string, double> movieDic = new Dictionary<string, double>();
            for(int i = 0; i < selectedlst.Count; i++)
            {
                int t = selectedlst[i].Genre.Count();
                for(int k = 0; k < t; k++)
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

            Console.WriteLine(averageGenreSC / sortedmovieDic.Count());
            Console.WriteLine(StdDev(gsArr));


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
                    foreach (KeyValuePair<string, double> tt in sortedmovieDic)
                    {
                        if (tt.Value > averageGenreSC / sortedmovieDic.Count()-StdDev(gsArr))
                            Fgenrelst.Add(tt.Key);
                    }
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

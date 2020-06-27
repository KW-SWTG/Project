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


using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using Microsoft.Toolkit.Wpf.UI.Controls;

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

            movieInfos = (List<MovieInfo>)Application.Current.Properties["mvInfoList2"];
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

        private void printReview()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCC13XkXYGADud9Wy5G_od0YL9r0mkVWAc",
                //AIzaSyC4MUT8FOREAZ7_i8ieWNiMgiv2QdMn7YU
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = db.title + "리뷰"; // Replace with your search term.
            searchListRequest.MaxResults = 50;
            searchListRequest.Fields = "items(id.videoId, snippet.channelId)";
            //searchListRequest.Type = "video";
            //searchListRequest.Fields = "items(id.videoId, snippet.channelId)";
            string[] channels =
            {
                "UCWcDLcxDu-cS9QD0IFMSzPw", //영화의 식탁
                "UCwR9k7QggFEfeHkvTR31JcA", // sk b tv
                "UCKBzr20577Xcn0LJoRVES_A", //무비띵크
                "UCBHXCaw_W6sxfgAB7rC-BYw", //소개해주는남자
                "UCClSUQVCidlSXUG679AfLbg", //달콤살벌 영화이야기
                "UCJU6Tal7hGSu7MdAmKVuj-Q", //리우군의 다락방
                "UCPptp7KX_0mpieCd7nwA-5A", //제이나나
                "UC3HH4dZj3CZmrCC0MuDliww", //영일남
                "UCu3BjLd03jxTVHXTPqZ77iQ", // 천재이승국
                "UCMguxwveCsLVpyKrLz-EFTg", //달빛뮤즈
                "UCwKR2UCcYD0TVDo0N9tN2Kg", //왓더무비괴수영화전문채널
                "UC8pFD_ZjCpifEZVK6vjV76Q", //영화리뷰하는여자
                "UCKFakQUa9S6TXO15xjYA1Eg", //거의없다
                "UC79hJz6y1EEiIkwfHOuWC4w", //김시선
                "UCZVr7RZO2qgaB8LYNwc7kkQ", //엉준
                "UCpCiIDf9UrfRqte55FHWlYQ", //드림텔러
                "UCxlv4aOnrRTXMRSL8bVJqEw", //삐맨
                "UCKNdfTZCJuOQfWN5Pe5UAAQ", //빨강도깨비
                "UCrBpV_pG2kyMMEHCMTNzjAQ", //리뷰엉이
                "UCC7bElwd29qpkgLo0j3l3Qw", //문화돋보기
                "UCZ6UWA7nCnH7h5Yn3XWp4Ig", //상됴리뷰
                "UCNR3K4HA6LyO9tz0oZoSJIA", //백수골방
                "UCiiC3grOarOVIg5LZgXJnWw", //민호타우ㄹㅡ스
                "UCb_GEWOGkCTXawWZKDtALwg", //9bul
                "UClgRkhTL3_hImCAmdLfDE4g", // 유튜브 영화 및 프로그램 코너.
                "UCNrkBmbHfEdwU-Gg6N-V0cw", //권잉튜브
                "UCoq-LkZU4n2tqdGEFUCwB1A", //카드값연체
                "UCJQEzHg0YOXp9q8P3hTy3yg", //중생이
                "UCLKYLWsKF4waqDx8T_43hBw", //유니버셜픽쳐스
                "UC6iL7bXdg0OF4ncDC27_Emg", //Ding tube
                "UCi8e0iOVk1fEOogdfu4YgfA", // Movieclips Trailers
                "UCvv65zhqR4ViA_N4QqLZ3BQ", // 뭐읽남
                "UCn0555uYXDVEkzC_roTa6_g", //조회수7777만회
                "UC6R-eEGhHVm4pv1cDohYB_w", //따따시영화리뷰
                "UCyi9eZCavFN5P6GQVokRiQg", //튜나
                "UCIXvXBYSc9fQ7Ri5SM1r8xA", //라이너의 컬쳐쇼크
                "UCJsyl3H1t62epJKQa5r5EWA", //5분다시보기
                "UCPpfTHg0QAHH4WqKZuLzimg", //기묘한케이지
                "UCsAoexnLeGwRym2DmjNRPUw", //두클립
                "UCLyZ3KMd8gBy4j8EEF13HjA", //꼰좌
                "UCiCEXFz9U8rNQn8G1ia3lgw", //영화보는낙타
                "UCnHfavZABe-QP17B-t3Pkew", //레츠워치
                "UCKdo6OzDVUneW7yPu4kdDvQ", //mbc plusm
                "UCuLvXhR22I-mErDiSCQECiQ", //왓칭맨
                "UCD311I2mw62niHiZibdp26Q", //초인
                "UCcJswosMito2Z7_ASgMrXpA", //영사관
                "UCZ5RU8OePo-Xzg9qCpXiCMA", //무비월드
                "UC0EV6YbRW6UHo-wtKQAD5PQ", //김흥미
                "UCiOWYRzOTiUYi9pJ-kscIKw", //발없는새
                "UC2p0WwXwUFj6Q-d62EKo78w", //영화대장
                "UCk9C0T1H6o9hYC3IrekfXZw", //고전찬미
                "UCJOTgAmY3S0mz-6imyhkj8g", //뭅뭅
                "UCSNYmoub8KzOFHc5U5E3q2w", //나인무비
                "UCyrPKxI_rxg3y5gLAEe4MDg", //비타무비
                "UCSJ-lEXwwasFewU5V2MrM9g", //영화끝무렵
                "UC3DNe5b3NYZ5ojre8YE1_xw", //진솔한 리뷰
                "UCVOjN9sB06uQkcm--3r6rBA", //영화소환사
                "UCNr6vPvJJhOi5X0AvrVbvBg", //비디오가게 시즌2
                "UC2NdmlV9bI7R33e1124Ao1w", //익스트림 무비
                "UCZLmL8OfgaKPcsJNTaikjFA", //B급 리뷰
                "UC7tpuimOq1oqpBqaWuHl3-g", //월요명화
                "UCDzpa0rTkUzxhE6h6fMKOQA", //자취방남자
                "UCEny3jH6oHGZ-_bL2Hpehew", //승헤이TV
                "UCOXAccjfcIvd-uRQgK2Fsrw", //늘보movie
                "UCNZXcxtJV3QxJUUGN9lFzTg", //DionysusCinema
                "UCn5EEmxMRcEPB7gwqcBdaXQ", //무비역쟁이
                "UCtwxuughKbSAV0e4bnz4ORg", //홍시네마
                "UCaWVEjVYBOSpB4gIHv78aFA", //영민하다
                "UCcfz-8gGDYJfaRHYD7kkQpw", //카랑
                "UC1riW5jynKVDiRE6IJ39spA", //시네한수
                "UC6J8MDOqR48rmazkrDFyzag", //곰줄
                "UCnRGNv84n0R-cp8kCoBy9Fg", //AHH
                "UC3NXMbLhNvu5m-6rz5uHCAg", //프리무비
                "UCQGTr4QurkCMJfVaZRiHB1Q", //나태
                "UCK6M78Rpw6-PHOYuoVrDqvA", //채널 휸
                "UCIqfDD5dq8rog3T5QonPDxg", //복학생아조씨의 영화소개
                "UCMWLDW_-5rD9FnQLzDpzExQ", //쿵덕무비
                "UClUFNgj9NonadDh6Mf4mFEg", //필름캐스터
                "UCVE-zq_yK_InhsVaJPQEttw", //캡틴라미
                "UCpr2S3SBmyjvrx9Q4pLUZHw", //무비트립
                "UC63OfFDn4I8AAhnqwr_bb1Q", //무비톡
                "UCHXH4PWrMwHcxxMzML-S0Ig", //씨네마 블라썸
                "UCmGsEXuAa0gL1uTm9Od_TYw", //무비셀라
                "UChEHqoHMqtXtkSRCZlkQzGA", //다시네마
                "UCaUROKsNkFxiZ_bttLdNMuw", //덕브라더스
                "UCRytxwcg5ROIFlkDy6cozFQ", //북부의왕
                "UCwcLqP3jKb6tJS5sJYWYIxg", //MOBEE
                "UC4Jc7nRQS5yGF-gyFwutXMw", //콜라냥
                "UCvXOq3KIULK1vmI8aD2Vl2w", //무비투어
                "UCX9BXlPhNY8mjXY5n-aiCjg", //무비가가
                "UCuH8eA3RPhQcOF_-3vRB51A", //리필드 재영
                "UCX4K7Pf1yifbdlVDbk3_hgw", //로튼애플 영화리뷰
                "UC5YzbS0ivcE1KlUK8CYg6bQ", //영리남
                "UCAWdNtPji5BQD2j563pZP_w", //헤더의터닝페이지
                "UCg76gdeKeDd0i-fBLNlxLGw", //명동선배
                "UCYYxgyVdRVwBtQkX8yWDGOg", //믹스무비
                "UCul4FTKARC-EaBq0e8UH7RA", //리뷰 Master
                "UCWRxGZ_hE8HnPC1UG7Ek82g", //무B곰
                "UCs9bgiMvJkS2CodV9OfeYcQ", //영화보는건데
                "UC4a9C90kI9QzTVx9HRdlsjw", //어쩌다 영화한편
                "UCp9YuJ_SccWHZZZ3ilj9atA", //무리남
                "UCvZZwaTc898a3aGYHL71m4w", //맛있는영화
                "UCMkLfngVvKjBtOl0mmE8-hw", //메기무비
                "UCiR3auXlnHVE6bcAUOar1yA", //오동키
                "UCvcz26JoycRhhQUSvLquU8A", //무비퓨레
                "UC7QZ9qyH5gZbx-QPmjIq5AA", //시리즈를 리뷰하다
                "UCCh1gXbyyn2x2oA_irIeguA", //두리무비
                "UCPIxlT2oA7KRRTZwQHnawAw", //프릭무비
                "UC4CUEWdb-poDaHTPDvf9N5g", //실연남
                "UCU8H8oODmFTD2YqFvCCdQ6w", //리드무비
                "UCRG64darCZhbsnsZCEiY2kQ", //Bucket studio
                "UCXvftM-43hGjGc1TA-8Y_IA", //동네비디오방
                "UCMWLDW_-5rD9FnQLzDpzExQ", //쿵덕무비
                "UCliriVEEXs-O08Imw9e7W_A", //아무튼영화
                "UCYYxgyVdRVwBtQkX8yWDGOg", //믹스무비
                "UCvVsKLeVE4OrxkrjjHNeyqQ", //리뷰몬스터
                "UCmR42q8aDhGM1U-drl6jnbw", //무비맨
                "UCunW2CJ2eA3T1BZZW-ErNAg", //무비웨이브스
                "UCuV63Rz4mE4YAagcIIxvbWw", //목소리이쁜언니
                "UCisEKsvUwJh2wayQZUcs2BQ", //하킹tv
                "UCnzKrg4wkqm_srS8Ek6r6hw", //어벙이무비
                "UCJrm4EOu7QdJC2W31jwW5Lw", //무비톡 two
                "UCwdJhYvBxVnWAjM0RYBmT4g", //영화보는하루
                "UCEI5S-nUp2YyYsp91zhygmg", //이지무비
                "UCS856BTCqsNXOXVgJJ_v4sg", //이집영화괜찮네
                "UCAxaLsT_FkWqr-3SfxQTjPA", //픽션월드
                "UCVt1IIz1P45h64V4hQ6xJmg", //코뮤
                "UCi-HVFYeDNfAZMLYIul9wOA", //찬호리뷰tv
                "UChlgI3UHCOnwUGzWzbJ3H5w",//Ytn news
            };
            var searchListResponse = searchListRequest.Execute();

            //var searchResultFirstRivew = searchListResponse.Items[0];
            //var searchResultSecondReview = searchListResponse.Items[1];


            bool bFound = false;
            foreach (var searchResult in searchListResponse.Items)
            {
                bFound = false;
                foreach (var chid in channels)
                {
                    if (chid == searchResult.Snippet.ChannelId)
                    {
                        bFound = true;
                        string review
                            = "<html><body marginheight=\"0\" marginwidth=\"0\" leftmargin=\"0\" topmargin=\"0\" style=\"overflow-y: hidden\"> " +
                            "<iframe width=\"515\" height=\"340\" src=\"https://www.youtube.com/embed/" + searchResult.Id.VideoId + "\" frameborder=\"0\" " +
                            "allow=\"accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen=\"ture\"></ifame>" +
                            "</body> </html>";
                        wvYoutubeReview.NavigateToString(review);
                        break;
                    }
                }
                if (bFound)
                {
                    break;
                }

            }
            if (!bFound)
            {
                string review = "<html><body><h2>리뷰를 찾을 수 없습니다.</h2></body></html>";
                wvYoutubeReview.NavigateToString(review);
            }
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (WebLib.UpdateMovieDB(webBrowser.Document as mshtml.HTMLDocument, db))
            {
                mvImg.Stretch = Stretch.Uniform;
                mvImg.Source = new BitmapImage(new Uri(db.posterUrl));

                printMovie();
                printRecommendMovies();
                printReview();
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
                wvYoutubeReview.Close();
                this.Close();
                Window1 newWindow = new Window1(name);
                newWindow.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wvYoutubeReview.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PROJECT
{
    class MovieInfo
    {
        private string movieName;       // 영화제목
        public string MovieName
        {
            get { return movieName; }
            set { if (value.Length > 0) movieName = value; }
        }

        private string url;             // 네이버 영화 url
        public string Url
        {
            get { return url; }
            set { if (value.Length > 0) url = value; }
        }

        public string moviePoster;             //top list Movies imageurls
        public string MoviePoster
        {
            get { return moviePoster; }
            set { if (value.Length > 0) moviePoster = value; }
        }

        private string[] genre;
        public string[] Genre
        {
            get { return genre;}
            set { if (value.Length > 0) genre = value; }
        }
    }

    class MovieDB
    {
        public string title = "";
        public string url = "";
        public string posterUrl = "";       // 포스터 url  
        public string audRating = "";       // 관객 평점
        public string expRating = "";       // 전문가 평점
        public string netRating = "";       // 네티즌 평점
        public string releaseDate = "";     // 개봉일
        public string runningTime = "";     // 상영 시간
        public string movieRating = "";     // 관람 등급
        public List<string> genre = new List<string>();     // 장르
        public List<string> nation = new List<string>();    // 국가
        public List<string> director = new List<string>();  // 감독들
        public List<string> actor = new List<string>();     // 배우들
        public List<string> recommendMovies = new List<string>();   // 유사한 영화들

        public bool Is()
        {
            if(title != "" && url != "")
            {
                return true;
            }
            return false;
        }
    }

    class  WebLib
    {
        static public bool findPoster(mshtml.HTMLDocument doc, MovieDB db)
        {
            if (db == null || doc == null)
                return false;
            
            mshtml.IHTMLElementCollection item = doc.getElementsByTagName("img");
            string posterUrl = "";

            int cnt = 0;
            foreach(mshtml.IHTMLElement elem in item)
            {
                if(elem.getAttribute("alt") == db.title && cnt == 0)
                {
                    posterUrl = elem.getAttribute("src");
                    break;
                }
            }

            if (posterUrl.Length <= 0)
                return false;

            db.posterUrl = posterUrl.Split('?')[0];

            return true;
        }

        static public void GetRatings(mshtml.HTMLDocument doc, MovieDB db)
        {
            mshtml.IHTMLElementCollection item = doc.getElementsByTagName("div");

            int lineIndex = 0;
            foreach(mshtml.IHTMLElement elem in item)
            {
                var score = elem.getAttribute("className");
                if(score == "star_score" || score == "star_score ")
                {
                    string text = elem.innerText;
                    text = text.Replace("\r\n", "");

                    if (lineIndex == 0)
                    {
                        text = text.Replace("관람객 평점 ", "");
                        text = text.Replace("점", ",");
                        db.audRating = text.Split(',')[0];
                    }
                    else if (lineIndex == 1)
                        db.expRating = text;
                    else if (lineIndex == 2)
                        db.netRating = text;

                    lineIndex++;
                    if (lineIndex >= 3)
                        break;
                }
            }
        }

        static public void GetRecommendMovies(mshtml.HTMLDocument doc, MovieDB db)
        {
            mshtml.IHTMLElementCollection item = doc.getElementsByTagName("ul");

            foreach(mshtml.IHTMLElement elem in item)
            {
                if(elem.getAttribute("className") == "thumb_link_mv")
                {
                    string text = elem.innerText;
                    text = text.Replace("\r\n", ",");
                    text = text.Replace(",,, ,", string.Empty);
                    text = text.Substring(4);

                    string[] movies = text.Split(',');
                    db.recommendMovies.AddRange(movies);

                    break;
                }
            }
        }

        static public bool UpdateMovieDB(mshtml.HTMLDocument doc, MovieDB db)
        {
            if (db == null || doc == null)
                return false;

            mshtml.IHTMLElementCollection item = doc.getElementsByTagName("dd");

            int lineIndex = 0;
            foreach(mshtml.IHTMLElement elem in item)
            {
                string text = elem.innerText;

                if(lineIndex == 0)
                {
                    // 감독
                    text = text.Replace(", ", ",");

                    string[] director = text.Split(',');
                    db.director.AddRange(director);
                }
                else if(lineIndex == 1)
                {
                    // 배우
                    text = text.Replace(", ", ",");

                    string[] actor = text.Split(',');
                    db.actor.AddRange(actor);
                }
                else if(lineIndex == 2)
                {
                    // 장르, 국가, 상영시간, 개봉일
                    text = text.Replace(", ", ",");
                    text = text.Replace("  ", "+");
                    text = text.Replace(" ", string.Empty);

                    string[] tmp = text.Split('+');

                    string[] genre = tmp[0].Split(',');
                    db.genre.AddRange(genre);

                    string[] nation = tmp[1].Split(',');
                    db.nation.AddRange(nation);

                    db.runningTime = tmp[2];
                    db.releaseDate = tmp[3];
                }
                else if(lineIndex == 5)
                {
                    // 관람 등급
                    text = text.Replace("\r\n", "");
                    text = text.Replace("도움말", "");
                    db.movieRating = text;
                    break;
                }

                lineIndex++;
            }

            // 유사한 영화들
            GetRecommendMovies(doc, db);

            // 관람객, 전문가, 네티즌 평점
            GetRatings(doc, db);

            return db.Is();
        }
    }

    class JsonLib
    {
        public static void InitMovieInfo(List<MovieInfo> movieInfos)
        {
            
            try
            {
                String str = System.IO.File.ReadAllText("TopMovieli.json");
                //기존 json list 에서 imageUrl키값을 추가하여 선택단계에서 사용함
                JArray jMovie = JArray.Parse(str);

                for(int i = 0; i < jMovie.Count; i++)
                {
                    MovieInfo tmp = new MovieInfo();
                    tmp.MovieName = jMovie[i]["MovieName"].ToString();
                    tmp.Url = jMovie[i]["Url"].ToString();
                    tmp.MoviePoster = jMovie[i]["ImageUrl"].ToString();
                    tmp.Genre = jMovie[i]["genre"].ToObject<string[]>();
                    movieInfos.Add(tmp);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            } 
        }

        public static string findMovieUrl(string title, List<MovieInfo> movieInfos)
        {
            string url = "";

            int findIndex = movieInfos.FindIndex(r => r.MovieName.Equals(title));
            if(findIndex < 0)
                MessageBox.Show("url을 찾을 수 없습니다");
            else
                url = movieInfos[findIndex].Url;

            return url;
        }

        public static string findPosterUrl(string title, List<MovieInfo> movieInfos)
        {
            string posterUrl = "";

            int findIndex = movieInfos.FindIndex(r => r.MovieName.Equals(title));
            if (findIndex < 0)
            {
                //MessageBox.Show("url을 찾을 수 없습니다");
            }
            else
                posterUrl = movieInfos[findIndex].moviePoster;

            return posterUrl;
        }
    }
}

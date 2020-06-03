using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PROJECT
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            List<MovieInfo> movieInfos = new List<MovieInfo>();
            JsonLib.SecondMovieInfo(movieInfos);
            List<MovieInfo> FmovieInfos = new List<MovieInfo>();
            JsonLib.InitMovieInfo(FmovieInfos);
            this.Properties["mvInfoList"] = movieInfos;
            this.Properties["mvInfoList2"] = FmovieInfos;

        }
    }
}

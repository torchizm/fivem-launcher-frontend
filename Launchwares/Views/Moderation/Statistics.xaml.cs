using DiscordRPC;
using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
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

namespace Launchwares.Views.Moderation
{
    /// <summary>
    /// Statistics.xaml etkileşim mantığı
    /// </summary>
    public partial class Statistics : Page
    {
        public Statistics(TimeScale Scale)
        {
            InitializeComponent();
            TimeScale.SelectedIndex = (int)Scale;
            GetDatas(Scale);
            TimeScale.SelectionChanged += TimeScale_SelectionChanged;
        }

        public async void GetDatas(TimeScale Timescale)
        {
            var LoginLogs = await API.client.Get<List<Models.Log>>($"logs/{API.client.Token.slug}?type=Game Login");
            var UpdateLogs = await API.client.Get<List<Models.Update>>($"updates/{API.client.Token.slug}");
            var CheatLogs = await API.client.Get<List<Models.Hack>>($"cheats/{API.client.Token.slug}");

            var NewUserValues = new ChartValues<double>();
            var LoginValues = new ChartValues<double>();
            var UpdateValues = new ChartValues<double>();
            var CheatValues = new ChartValues<double>();
            List<string> AxisX = new List<string>();

            switch (Timescale) {
                case Moderation.TimeScale.Monthly:
                    for (int i = 0; i < 12; i++) {
                        var monthlytime = new DateTime(DateTime.UtcNow.Year, 1, 1).AddMonths(i);
                        var newusercount = Utils.Users.Count(x => x.JoinedDate.Year == monthlytime.Year && x.JoinedDate.Month == monthlytime.Month);
                        var logincount = LoginLogs.Count(x => x.Date.Year == monthlytime.Year && x.Date.Month == monthlytime.Month);
                        var updatecount = UpdateLogs.Count(x => x.Date.Year == monthlytime.Year && x.Date.Month == monthlytime.Month);
                        var cheatcount = CheatLogs.Count(x => x.Date.Year == monthlytime.Year && x.Date.Month == monthlytime.Month);
                        if (logincount != 0 || newusercount != 0) {
                            NewUserValues.Add(newusercount);
                            LoginValues.Add(logincount);
                            UpdateValues.Add(updatecount);
                            CheatValues.Add(cheatcount);
                            AxisX.Add(monthlytime.ToString("MMMM"));
                        }
                    }
                    Labels = AxisX.ToArray();
                    break;

                case Moderation.TimeScale.Weekly:
                    var weeklytime = DateTime.UtcNow.AddDays(-21);
                    for (int i = 0; i < 4; i++) {
                        var newusercount = Utils.Users.Count(x => DateHelper.GetWeekNumberOfMonth(x.JoinedDate) == DateHelper.GetWeekNumberOfMonth(weeklytime));
                        var logincount = LoginLogs.Count(x => DateHelper.GetWeekNumberOfMonth(x.Date) == DateHelper.GetWeekNumberOfMonth(weeklytime));
                        var updatecount = UpdateLogs.Count(x => DateHelper.GetWeekNumberOfMonth(x.Date) == DateHelper.GetWeekNumberOfMonth(weeklytime));
                        var cheatcount = CheatLogs.Count(x => DateHelper.GetWeekNumberOfMonth(x.Date) == DateHelper.GetWeekNumberOfMonth(weeklytime));
                        NewUserValues.Add(newusercount);
                        LoginValues.Add(logincount);
                        UpdateValues.Add(updatecount);
                        CheatValues.Add(cheatcount);
                        AxisX.Add($"{weeklytime:MMMM yyyy} - {DateHelper.GetWeekNumberOfMonth(weeklytime)}. {Application.Current.Resources["date.week"]}");
                        weeklytime = weeklytime.AddDays(7);
                    }
                    Labels = AxisX.ToArray();
                    break;

                case Moderation.TimeScale.Daily:
                    var dailytime = DateTime.UtcNow.AddDays(-30);
                    for (int i = 0; i <= 30; i++) {
                        var newusercount = Utils.Users.Count(x => x.JoinedDate.Year == dailytime.Year && x.JoinedDate.Month == dailytime.Month && x.JoinedDate.Day == dailytime.Day);
                        var logincount = LoginLogs.Count(x => x.Date.Year == dailytime.Year && x.Date.Month == dailytime.Date.Month && x.Date.Day == dailytime.Day);
                        var updatecount = UpdateLogs.Count(x => x.Date.Year == dailytime.Year && x.Date.Month == dailytime.Date.Month && x.Date.Day == dailytime.Day);
                        var cheatcount = CheatLogs.Count(x => x.Date.Year == dailytime.Year && x.Date.Month == dailytime.Date.Month && x.Date.Day == dailytime.Day);
                        NewUserValues.Add(newusercount);
                        LoginValues.Add(logincount);
                        UpdateValues.Add(updatecount);
                        CheatValues.Add(cheatcount);
                        AxisX.Add($"{dailytime:dd/MM/yyyy}");
                        dailytime = dailytime.AddDays(1);
                    }
                    Labels = AxisX.ToArray();
                    break;
            };

            UserStatsCollection = new SeriesCollection
            {
                new LineSeries {
                    Title = $"{Application.Current.Resources["statistics.usercount"]}",
                    Values = NewUserValues
                },
                new LineSeries {
                    Title = $"{Application.Current.Resources["statistics.totaldailylogin"]}",
                    Values = LoginValues
                }
            };

            UpdateStatsCollection = new SeriesCollection
            {
                new LineSeries {
                    Title = $"{Application.Current.Resources["statistics.updates"]}",
                    Values = UpdateValues
                }
            };

            CheatStatsCollection = new SeriesCollection
            {
                new LineSeries {
                    Title = $"{Application.Current.Resources["statistics.cheats"]}",
                    Values = CheatValues
                }
            };

            DataContext = this;
        }

        public SeriesCollection UserStatsCollection { get; set; }
        public SeriesCollection UpdateStatsCollection { get; set; }
        public SeriesCollection CheatStatsCollection { get; set; }
        public string[] Labels { get; set; }

        private void TimeScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.main.CreateView(new Statistics((TimeScale)this.TimeScale.SelectedIndex), MainWindow.main.ServerStatsButton, false);
        }
    }

    public enum TimeScale
    {
        Monthly = 0,
        Weekly = 1,
        Daily = 2
    }
}

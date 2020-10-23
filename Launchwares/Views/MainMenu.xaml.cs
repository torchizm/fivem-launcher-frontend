using Launchwares.AnimationHelpers;
using Launchwares.Core;
using Launchwares.Core.Antihack;
using Launchwares.CustomElements;
using Launchwares.Discord;
using LaunchwaresCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.MessageBox;
using MessageBoxIcon = System.Windows.MessageBoxImage;
using Path = System.IO.Path;
using Timer = System.Timers.Timer;
using Cursors = System.Windows.Input.Cursors;

namespace Launchwares.Views
{
    /// <summary>
    /// MainMenu.xaml etkileşim mantığı
    /// </summary>
    public partial class MainMenu : Page
    {
        int Players = 0;
        long Ping = 0;

        private Timer OnlineCountTimer = new Timer() {
            Interval = 20
        };

        private Timer PingCountTimer = new Timer() {
            Interval = 20
        };

        public MainMenu()
        {
            InitializeComponent();
        }

        bool LOADED = false;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Utils.Server.Maintenance) {
                if(Utils.UserType >= Models.UserType.Guider) {
                    PlayButton.Content = $"{System.Windows.Application.Current.Resources["application.maintenance"]}";
                }
                else {
                    GameGrid.Cursor = Cursors.No;
                    PlayButton.Background = Brushes.IndianRed;
                    PlayButton.IsEnabled = false;
                    PlayButton.Content = $"{System.Windows.Application.Current.Resources["application.maintenance"]}";
                }
            }

            OnlineCountTimer.Elapsed += OnlineCountTimer_Elapsed;
            PingCountTimer.Elapsed += PingCountTimer_Elapsed;
            GetServerInfo();

            var updates = await API.client.Get<List<Models.Update>>($"updates/{API.client.Token.slug}");

            if (updates != null) {
                int updatecount = (updates.Count > 5) ? 5 : updates.Count;
                for (int i = 0; i < updatecount; i++)
                    this.Dispatcher.Invoke(delegate
                    {
                        UpdateContainer.Children.Add(new MiniUpdateBox(updates[i]));
                    });
            }

            var posts = await API.client.Get<List<Models.Post>>($"posts/{API.client.Token.slug}");
            if (posts != null && posts.Count >= 2) {
                RandomPost.Children.Add(new PostBox(posts[0]));
                RandomPost.Children.Add(new PostBox(posts[1]));
            }
        }

        private async void GetServerInfo()
        {
            if (await CheckOnline(Utils.Server.ServerIp, Utils.Server.ServerPort))
                GetOnline(Utils.Server.ServerIp, Utils.Server.ServerPort);
        }

        public async void GetOnline(string ip, int? port)
        {
            try {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                string json = "";
                using (WebClient client = new WebClient()) {
                    json = await client.DownloadStringTaskAsync("http://" + ip + ":" + port + "/players.json");
                }

                List<Models.FivemPlayer> players = JsonConvert.DeserializeObject<List<Models.FivemPlayer>>(json);

                if (players != null && players.Count > 0) {
                    Players = players.Count;
                    Contents.PlayingCount = players.Count;
                    OnlineCountTimer.Start();
                    RichPresence.UpdatePresence();
                }
                else {
                    Players = 0;
                    Contents.PlayingCount = 0;
                    RichPresence.UpdatePresence();
                }
            }
            catch (Exception) { }
        }

        private async Task<bool> CheckOnline(string ip, int? port)
        {
            bool tcpCheck = false;

            using (TcpClient tcpClient = new TcpClient()) {
                try {
                    Ping myPing = new Ping();

                    PingReply reply = await myPing.SendPingAsync(ip);

                    if (reply != null) Ping = reply.RoundtripTime;

                    await tcpClient.ConnectAsync(ip, Convert.ToInt32(port));
                    tcpCheck = true;
                }
                catch (Exception) { return false; }
            }

            LOADED = tcpCheck;

            if (tcpCheck == true) {
                if (Ping > 0) PingCountTimer.Start();
                return true;
            }
            else
                return false;
        }

        private Timer FivemStartTimer = new Timer() {
            Interval = 1000
        };
        int sti = 0;

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (Utils.Server.DiscordWhitelist) {
                    var whitelist = await API.client.CustomPost<Models.GetResponse>($"http://whitelist.launchwares.com/api/check_whitelist?uid={Utils.Uid}");
                    if (Properties.Settings.Default.ManuallyCreated == false && whitelist.Response == false) {
                        MessageBox.Show("Bu sunucuda oynamak için whitelist yetkiniz yok.",
                                $"{System.Windows.Application.Current.Resources["application.title"]}",
                                MessageBoxButton.OK,
                                MessageBoxIcon.Exclamation);
                        return;
                    }
                    else {
                        Utils.Whitelist = true;
                    }
                }

                if (Utils.Whitelist == false) {
                    MessageBox.Show("Bu sunucuda oynamak için whitelist yetkiniz yok.",
                        $"{System.Windows.Application.Current.Resources["application.title"]}",
                        MessageBoxButton.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }

            if (LOADED) {
                //if (!Properties.Settings.Default.ManuallyCreated
                //    && RichPresence.client.CurrentPresence != null)
                //    RichPresence.client.ClearPresence();

                await API.client.Post<string>($"launcher_login/{API.client.Token.slug}?uid={Utils.Uid}");
                MainWindow.main.ShowInTaskbar = false;
                MainWindow.main.WindowState = WindowState.Minimized;

                string TeamspeakConnection = Utils.Server.TeamspeakIp != ""
                                             ? TeamspeakConnection = $"ts3server://{Utils.Server.TeamspeakIp}"
                                             : TeamspeakConnection = "";

                if (Utils.Server.TeamspeakPort != null && Utils.Server.TeamspeakPort != 0)
                    TeamspeakConnection += $":{Utils.Server.TeamspeakPort}";

                if (Utils.Server.TeamspeakPassword != "")
                    TeamspeakConnection += $"?password={Utils.Server.TeamspeakPassword}";

                ProcessStartInfo StartTeamspeak = new ProcessStartInfo(TeamspeakConnection);
                if (Utils.Server.TeamspeakIp == "") TeamspeakConnection = "";
                if (TeamspeakConnection != "")
                    StartTeamspeak = new ProcessStartInfo(TeamspeakConnection) {
                        WindowStyle = ProcessWindowStyle.Minimized
                    };

                if (Process.GetProcessesByName("ts3client_win64").Length == 0
                    && TeamspeakConnection != "")
                    Process.Start(StartTeamspeak);

                try {
                    if (Properties.Settings.Default.Location_FiveM != "" &&
                        !File.Exists($"{Properties.Settings.Default.Location_FiveM}\\FiveM.exe")) {
                        SetFivemLocation();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Fivem lokasyonunu ararken bir hata meydana geldi. Hata detaylarını lütfen yetkili kişiye iletiniz;\n{ex.ToString()}", $"{System.Windows.Application.Current.Resources["application.title"]}");
                }

                Library.HackTimer.Stop();
                string StartArgs = $"/C title FiveM Bootstrapper&\"{Properties.Settings.Default.Location_FiveM}\\FiveM.exe\" +connect {Utils.Server.ServerIp}";
                ProcessStartInfo Fivem = new ProcessStartInfo("cmd.exe", StartArgs);
                Fivem.WindowStyle = ProcessWindowStyle.Hidden;
                Fivem.CreateNoWindow = true;
                Fivem.UseShellExecute = false;

                if (Utils.Server.ServerPort != null && Utils.Server.ServerPort != 0) Fivem.Arguments += $":{Utils.Server.ServerPort}";
                Process.Start(Fivem);

                FivemStartTimer.Elapsed += FivemStartTimer_Elapsed;
                FivemStartTimer.Start();
            }
        }

        private void FivemStartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sti == 10) {
                foreach (Process process in Process.GetProcesses()) {
                    if (process.ProcessName.ToLower() == "cmd") {
                        process.Kill();
                    }
                }
            }
            if (sti == 15) {
                Library.StartHackTimer();
                FivemStartTimer.Stop();
            }
        }

        public void SetGtaVLocation()
        {
            FolderBrowserDialog gtavlocation = new FolderBrowserDialog() {
                Description = $"{System.Windows.Application.Current.Resources["mainmenu.selectgtavlocation"]}"
            };
            DialogResult result = gtavlocation.ShowDialog();
            if (result == DialogResult.OK)
                if (File.Exists($@"{gtavlocation.SelectedPath}\GTA5.exe"))
                    SetLocation(gtavlocation.SelectedPath, "gtav");
                else
                    MessageBox.Show($"{System.Windows.Application.Current.Resources["mainmenu.nogtavatlocation"]}",
                                    $"{System.Windows.Application.Current.Resources["application.title"]}",
                                    MessageBoxButton.OK,
                                    MessageBoxIcon.Error);
        }

        public void SetFivemLocation()
        {
            FolderBrowserDialog fivemlocation = new FolderBrowserDialog() {
                Description = $"{System.Windows.Application.Current.Resources["mainmenu.selectfivemlocation"]}"
            };
            DialogResult result = fivemlocation.ShowDialog();
            if (result == DialogResult.OK)
                if (File.Exists($@"{fivemlocation.SelectedPath}\FiveM.exe"))
                    SetLocation(fivemlocation.SelectedPath, "fivem");
                else
                    MessageBox.Show($"{System.Windows.Application.Current.Resources["mainmenu.nofivematlocation"]}",
                                    $"{System.Windows.Application.Current.Resources["application.title"]}",
                                    MessageBoxButton.OK,
                                    MessageBoxIcon.Error);
        }

        private void SetLocation(string path, string type)
        {
            if (type == "gtav") {
                Properties.Settings.Default.Location_GtaV = path;
                Properties.Settings.Default.Save();
            }
            else if (type == "fivem") {
                Properties.Settings.Default.Location_FiveM = path;
                Properties.Settings.Default.Save();
            }
        }

        private void PingCountTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(delegate
            {
                if (Convert.ToInt32(PingCountLabel.Text) > Ping - 10) PingCountTimer.Interval += 20;
                PingCountLabel.Text = (Convert.ToInt32(PingCountLabel.Text) + 1).ToString();
                if (PingCountLabel.Text == Ping.ToString()) PingCountTimer.Stop();
            });
        }

        private void OnlineCountTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(delegate
            {
                if (Convert.ToInt32(OnlineCountLabel.Text) > Players - 10) OnlineCountTimer.Interval += 20;
                OnlineCountLabel.Text = (Convert.ToInt32(OnlineCountLabel.Text) + 1).ToString();
                if (OnlineCountLabel.Text == Players.ToString()) OnlineCountTimer.Stop();
            });
        }

        private void ToLeft_Click(object sender, RoutedEventArgs e) => ScrollViewerHelper.ScrollToHorizontalPosition(UpdatesScrollViewer, UpdatesScrollViewer.HorizontalOffset - 460);

        private void ToRight_Click(object sender, RoutedEventArgs e) => ScrollViewerHelper.ScrollToHorizontalPosition(UpdatesScrollViewer, UpdatesScrollViewer.HorizontalOffset + 460);

        private void UpdatesScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ToLeft.Visibility = (UpdatesScrollViewer.HorizontalOffset == 0) ? Visibility.Hidden : Visibility.Visible;
            ToRight.Visibility = (UpdatesScrollViewer.HorizontalOffset == UpdatesScrollViewer.ScrollableWidth) ? Visibility.Hidden : Visibility.Visible;
        }
    }
}

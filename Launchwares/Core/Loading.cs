using Launchwares.Helpers;
using LaunchwaresCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Launchwares.Core
{
    class Loading
    {
        static System.Windows.Forms.Timer TokenWaiter = new System.Windows.Forms.Timer() {
            Interval = 1000
        };

        static int RPCErrorWaiter { get; set; } = 0;
        static System.Windows.Forms.Timer RPCWaiter = new System.Windows.Forms.Timer() {
            Interval = 1000
        };

        public static async void CheckVersion()
        {
            var serverInfo = await API.client.Get<Models.ServerInfo>($"server?a=b", true);
            serverInfo.LogoPath = $"{API.client.ApiPath.Substring(0, API.client.ApiPath.Length - 3)}storage/{serverInfo.LogoPath}";
            Application.Current.Resources["application.title"] = serverInfo.Name;
            MainWindow.main.LoadingImage.ImageSource = ImageHelper.ConvertPhoto(serverInfo.LogoPath);
            MainWindow.main.NavbarPhoto.ImageSource = ImageHelper.ConvertPhoto(serverInfo.LogoPath);
            MainWindow.main.Icon = ImageHelper.ConvertPhoto(serverInfo.LogoPath);

            TokenWaiter.Tick += TokenWaiter_Tick;
            RPCWaiter.Tick += RPCWaiter_Tick;
            API.client.OnTokenSync += Client_OnTokenSync;

            MainWindow.main.LoadingText.Text = $"{Application.Current.Resources["loading.connectingdiscord"]}";

            if (!Properties.Settings.Default.ManuallyCreated) {
                var Subproc = new SubprocessController(
                    $@"{AppDomain.CurrentDomain.BaseDirectory}LaunchwaresSubprocess.exe",
                    $"-rpc_id {Properties.Settings.Default.RPCId} -rpc_description {Properties.Settings.Default.Description} -rpc_largeimage_key {Properties.Settings.Default.RPCLargeImageKey} -rpc_largeimage_text {Properties.Settings.Default.RPCLargeImageText} -rpc_smallimage_key {Properties.Settings.Default.RPCSmallImageKey} -rpc_smallimage_text {Properties.Settings.Default.RPCSmallImageText} -max_players {Properties.Settings.Default.MaxPlayers} -now_playing 1 -testkey testvalue");
                Subproc.OutputDataReceived += Subproc_OutputDataReceived;

                RPCWaiter.Start();
            }
            else {
                Utils.Username = Properties.Settings.Default.PlayerUsername;
                Utils.Uid = Properties.Settings.Default.PlayerUID;
                Utils.ProfilePhoto = Properties.Settings.Default.PlayerProfilephotoPath;
                API.GetToken();
                TokenWaiter.Start();
            }
        }

        private static void Subproc_OutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            var User = JsonConvert.DeserializeObject<dynamic>(args.Data);
            Utils.Uid = Convert.ToUInt64(User.Uid);
            Utils.ProfilePhoto = Convert.ToString(User.ProfilePhoto);
            Utils.Username = Convert.ToString(User.Username);

            Properties.Settings.Default.PlayerUID = Utils.Uid;
            Properties.Settings.Default.PlayerUsername = Utils.Username;
            Properties.Settings.Default.PlayerProfilephotoPath = Utils.ProfilePhoto;
            Properties.Settings.Default.Save();
        }

        private static void RPCWaiter_Tick(object sender, EventArgs e)
        {
            if (Utils.ProfilePhoto != null &&
                Utils.Username != null &&
                Utils.Uid != 0) {
                RPCWaiter.Stop();
                API.GetToken();
                TokenWaiter.Start();
            }
            else {
                RPCErrorWaiter++;
                MainWindow.main.LoadingText.Text = $"{Application.Current.Resources["loading.connectingdiscord"]} ({RPCErrorWaiter})";
                if (RPCErrorWaiter == 15) {
                    MainWindow.main.SmallIcon.Visible = false;
                    MainWindow.main.LoadingContainer.Visibility = Visibility.Hidden;
                    MainWindow.main.RegisterContainer.Visibility = Visibility.Visible;
                }
            }
        }

        private static void TokenWaiter_Tick(object sender, EventArgs e)
        {
            MainWindow.main.LoadingText.Text = $"{Application.Current.Resources["loading.connectingservices"]}";

            if (API.client.Token != null && API.client.Token.slug != null) {
                RPCLoaded();
                TokenWaiter.Stop();
            }
        }

        private async static void Client_OnTokenSync(Models.Token source)
        {
            var server = await API.client.Get<Models.Server>($"server/{API.client.Token.slug}");
            Utils.Server = server;

            if (server.LogoPath != "" && server.LogoPath != Properties.Settings.Default.LogoPath) {
                Properties.Settings.Default.LogoPath = $"{API.client.ApiPath.Substring(0, API.client.ApiPath.Length - 3)}storage/{server.LogoPath}";
                Properties.Settings.Default.Save();
            }
            if (server.MaxPlayers != Properties.Settings.Default.MaxPlayers) {
                Properties.Settings.Default.MaxPlayers = Convert.ToInt32(server.MaxPlayers);
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.RPCId != server.RpcId) {
                Properties.Settings.Default.RPCId = server.RpcId;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.RPCLargeImageKey != server.RpcLargeImageKey) {
                Properties.Settings.Default.RPCLargeImageKey = server.RpcLargeImageKey;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.RPCLargeImageText != server.RpcLargeImageText) {
                Properties.Settings.Default.RPCLargeImageText = server.RpcLargeImageText;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.RPCSmallImageKey != server.RpcSmallImageKey) {
                Properties.Settings.Default.RPCSmallImageKey = server.RpcSmallImageKey;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.RPCSmallImageText != server.RpcSmallImageText) {
                Properties.Settings.Default.RPCSmallImageText = server.RpcSmallImageText;
                Properties.Settings.Default.Save();
            }
            if (server.Description != Properties.Settings.Default.Description) {
                Properties.Settings.Default.Description = server.Description;
                Properties.Settings.Default.Save();
            }
            if (server.ThemeIndex != Properties.Settings.Default.ThemeIndex && Properties.Settings.Default.UserThemeIndex == false) {
                Properties.Settings.Default.ThemeIndex = server.ThemeIndex;
                Properties.Settings.Default.Save();
            }

            try {
                Utils.Hashes = await API.client.Get<List<Models.Hash>>("hashes");
                var blacklist = await API.client.Get<Models.Blacklist>($"blacklist");
                Utils.IllegalPrograms = blacklist.Programs;

                DetectFiles();
            }
            catch (Exception) { }

            if (API.client.Token.version != Utils.Version) {
                //new SubprocessController($"{AppDomain.CurrentDomain.BaseDirectory}LaunchwaresUpdater.exe",
                //    $"-token {API.client.Token.slug} -name {Application.Current.Resources["application.title"]} -photopath {Properties.Settings.Default.LogoPath} -lang {Properties.Settings.Default.Language} -t");
                ProcessStartInfo updater = new ProcessStartInfo($"{ AppDomain.CurrentDomain.BaseDirectory }LaunchwaresUpdater.exe") {
                    Arguments = $"-token {API.client.Token.slug} -name {Application.Current.Resources["application.title"]} -photopath {Properties.Settings.Default.LogoPath} -lang {Properties.Settings.Default.Language} -t",
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process.Start(updater);

                System.Timers.Timer t = new System.Timers.Timer() {
                    Interval = 300
                };

                t.Elapsed += (sender, arg) => Environment.Exit(0);
                t.Start();
            }
        }

        internal async static void RPCLoaded()
        {
            var user = await API.client.Get<Models.User>($"player/{API.client.Token.slug}?player={Utils.Uid}");

            MessageBox.Show(JsonConvert.SerializeObject(user));

            Utils.Status = (Models.Status)user.Status;
            Utils.UserType = (Models.UserType)user.Usertype;
            Utils.Whitelist = user.Whitelist;

            MainWindow.main.UsernameLabel.Text = Utils.Username;
            MainWindow.main.UserPhoto.ImageSource = ImageHelper.ConvertPhoto(Utils.ProfilePhoto);
            Utils.Users = await API.client.Get<List<Models.User>>($"players/{API.client.Token.slug}");
        }

        public static void DetectFiles()
        {
            MainWindow.main.LoadingText.Text = $"{Application.Current.Resources["loading.checkingfiles"]}";

            if (Properties.Settings.Default.Location_FiveM == "") {
                //string fivemLocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/FiveM";
                var fileFullLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fivem", "Fivem.Exe");

                if (File.Exists(fileFullLocation)) {
                    while (true)
                        if (Utils.SetFivemLocation()) {
                            MainWindow.main.LauncherLoaded();
                            break;
                        }
                }
            } else {
                var f = new FileInfo($@"{Properties.Settings.Default.Location_FiveM}\FiveM.exe");

                if (f.Exists && f.Length > (Utils.Server.ByteCount - 20000) && f.Length < Utils.Server.ByteCount + 20000) {
                    MainWindow.main.LauncherLoaded();
                }
                else {
                    while (true) {
                        if (Utils.SetFivemLocation()) {
                            MainWindow.main.LauncherLoaded();
                            break;
                        }
                    }
                }
            }
        }
    }
}

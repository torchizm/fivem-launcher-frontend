using Launchwares.Helpers;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Launchwares.Core.Anticheat
{
    internal static class Library
    {
        internal static Timer HackTimer = new Timer() {
            Interval = 5000,
            Enabled = false
        };

        internal static bool HackTimerStarted = false;
        internal static bool LastInternetStatus = true;

        internal static Data IsHackAsync()
        {
            if (Utils.Uid == 0 && Utils.Username == null) return null;
            if (Utils.IllegalPrograms == null) return null;

            foreach (Process process in Process.GetProcesses()) {
                if (Utils.IllegalPrograms.Contains(process.ProcessName.ToLower()) || process.MainWindowTitle.Trim() == "NUI DevTools") {
                    new SubprocessController($@"{AppDomain.CurrentDomain.BaseDirectory}\LaunchwaresSubprocess.exe",
                        $"-kill_program {process.ProcessName} -");

                    var CheatData = new Data {
                        Date = DateTime.Now.ToString(),
                        ClientType = (process.MainWindowTitle.Trim() == "NUI Devtools") ? process.ProcessName : "nui devtools",
                        DiscordUID = Utils.Uid,
                        DiscordUserName = Utils.Username
                    };

                    byte[] ScreenshotBytes = new ScreenCapture().CaptureScreenBytes();
                    PostDetection(CheatData, ScreenshotBytes);
                    return CheatData;
                }
            }

            //if (!Utils.IsInternetOn()) InternetOff();
            //else if (Utils.IsInternetOn() && LastInternetStatus == true) InternetOn();
            return null;
        }

        public static void InternetOff()
        {
            MainWindow.main.ProgramContainer.Visibility = Visibility.Hidden;
            MainWindow.main.CheatTitleText.Text = $"{System.Windows.Application.Current.Resources["application.internetoff"]}";
            MainWindow.main.CheatContainer.Visibility = Visibility.Visible;
            Utils.ClosePrograms();
        }

        public static void InternetOn()
        {
            MainWindow.main.CheatContainer.Visibility = Visibility.Hidden;
            MainWindow.main.ProgramContainer.Visibility = Visibility.Visible;
        }

        public async static void PostDetection(Data CheatData, byte[] ScreenshotBytes = null, string ScreenshotPath = null, bool ShowScreen = true)
        {
            int clientIndex = 0;
            for (int i = 0; i < Utils.IllegalPrograms.Count; i++) {
                if (Utils.IllegalPrograms[i] == CheatData.ClientType.ToLower()) {
                    clientIndex = i + 1;
                    break;
                }
            }

            if (ShowScreen) {
                MainWindow.main.ProgramContainer.Visibility = Visibility.Hidden;
                MainWindow.main.CheatTitleText.Text = Application.Current.Resources["anticheat.information"].ToString().Replace("%CheatName%", CheatData.ClientType);
                MainWindow.main.CheatContainer.Visibility = Visibility.Visible;
                Utils.ClosePrograms();
            }

            if (ScreenshotBytes != null)
                ScreenshotPath = await API.client.UploadImageWithoutPath(ScreenshotBytes);

            await API.client.Post<Models.Hack>($"detection/{API.client.Token.slug}?cheat={clientIndex}&player={Utils.Uid}&screenshot_path={ScreenshotPath}&details={CheatData.Details}");
            return;
        }

        public static void StartHackTimer()
        {
            HackTimer.Start();
            if (!HackTimerStarted) HackTimer.Tick += HackTimer_Tick;
            HackTimerStarted = true;
        }

        private static void HackTimer_Tick(object sender, EventArgs e) => IsHackAsync();

        public static async void CheckGameFiles()
        {
            var fivemPath = Properties.Settings.Default.Location_FiveM;
            var dataPath = Path.Combine(fivemPath, "FiveM.App", "citizen", "common", "data");
            var ai = new DirectoryInfo(Path.Combine(dataPath, "ai"));

            if (ai.Exists) {
                var fs = ai.GetFiles().ToList();
                List<string> cf = new List<string>();

                foreach (var f in fs) {
                    var fileText = File.ReadAllText(f.FullName);
                    string md5 = CryptographyHelper.CreateMD5(fileText);

                    if (Utils.Hashes.Any(x => x.EncryptedString == md5)) {
                        ;
                    }
                    else {
                        cf.Add(f.Name);
                        var uploadedPath = await API.client.UploadFileWithoutPath(File.ReadAllBytes(f.FullName));
                        var data = new Data() {
                            ClientType = $"illegal-file-found",
                            Date = DateTime.Now.ToString(),
                            DiscordUID = Utils.Uid,
                            DiscordUserName = Utils.Username,
                            Details = $"Dosya: {f.FullName}\r\nOluşturulma: {f.CreationTime}\r\nSon erişim: {f.LastAccessTime}\r\nSon düzenleme:{f.LastWriteTime}"
                        };

                        PostDetection(data, null, uploadedPath, false);
                    }
                }

                if (cf.Count != 0) {
                    string fString = "";

                    foreach (var f in cf)
                        if (f != cf.LastOrDefault())
                            fString += $"{f}, ";
                        else
                            fString += $"{f}";

                    MainWindow.main.ProgramContainer.Visibility = Visibility.Hidden;
                    MainWindow.main.CheatTitleText.Text = Application.Current.Resources["anticheat.foundillegalfiles"].ToString().Replace("%FileNames%", fString);
                    MainWindow.main.CheatContainer.Visibility = Visibility.Visible;
                    Utils.ClosePrograms();
                }
            }
        }
    }
}
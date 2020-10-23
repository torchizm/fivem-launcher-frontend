using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Launchwares.Core.Antihack
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
                if (Utils.IllegalPrograms.Contains(process.ProcessName.ToLower()) || process.MainWindowTitle.ToLower() == "nui devtools") {
                    new SubprocessController($@"{AppDomain.CurrentDomain.BaseDirectory}\LaunchwaresSubprocess.exe",
                        $"-kill_program {process.ProcessName} -");

                    var CheatData = new Data {
                        Date = DateTime.Now.ToString(),
                        ClientType = (process.MainWindowTitle.ToLower() == "nui devtools") ? process.ProcessName : "nui devtools",
                        DiscordUID = Utils.Uid,
                        DiscordUserName = Utils.Username
                    };

                    byte[] ScreenshotBytes = new ScreenCapture().CaptureScreenBytes();
                    PostDetection(CheatData, ScreenshotBytes);
                    return CheatData;
                }
            }

            if (!Utils.IsInternetOn()) InternetOff();
            else if (Utils.IsInternetOn() && LastInternetStatus == true) InternetOn();
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

        public async static void PostDetection(Data CheatData, byte[] ScreenshotBytes)
        {
            int clientIndex = 0;
            for (int i = 0; i < Utils.IllegalPrograms.Count; i++) {
                if (Utils.IllegalPrograms[i] == CheatData.ClientType.ToLower()) {
                    clientIndex = i + 1;
                    break;
                }
            }

            MainWindow.main.ProgramContainer.Visibility = Visibility.Hidden;
            MainWindow.main.CheatTitleText.Text = MainWindow.main.CheatTitleText.Text.Replace("%CheatName%", CheatData.ClientType);
            MainWindow.main.CheatContainer.Visibility = Visibility.Visible;
            Utils.ClosePrograms();

            var ScreenshotPath = await API.client.UploadImageWithoutPath(ScreenshotBytes);
            await API.client.Post<Models.Hack>($"detection/{API.client.Token.slug}?cheat={clientIndex}&player={Utils.Uid}&screenshot_path={ScreenshotPath}");
            return;
        }

        public static void StartHackTimer()
        {
            HackTimer.Start();
            if(!HackTimerStarted) HackTimer.Tick += HackTimer_Tick;
            HackTimerStarted = true;
        }

        private static void HackTimer_Tick(object sender, EventArgs e) => IsHackAsync();
    }
}
using Launchwares.Core.Anticheat;
using Launchwares.Helpers;
using LaunchwaresCore;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using MessageBoxButton = System.Windows.Forms.MessageBoxButtons;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using DialogResult = System.Windows.Forms.DialogResult;
using FolderDialogResult = System.Windows.Forms.DialogResult;
using System;

namespace Launchwares.Core
{
    class Utils
    {
        internal static Models.Server Server { get; set; }
        public static List<Models.User> Users { get; set; }
        //public static List<Models.Message> MessageCache = new List<Models.Message>();
        //public static List<int> MessageCacheIDs = new List<int>();
        public static List<Models.Hash> Hashes { get; set; }
        public static IList<string> IllegalPrograms { get; set; }
        public static string Version = "1.0.3.3";
        public static string Username { get; set; }
        public static ulong Uid { get; set; }
        public static string ProfilePhoto { get; set; }
        public static Models.Status Status { get; set; }
        public static Models.UserType UserType { get; set; }
        public static bool Whitelist { get; set; }
        public static ResourceDictionary Language { get; set; }

        public static string GetUsername(ulong id) => Users.Where(x => x.Uid == id).Select(x => x.Username).FirstOrDefault() ?? $"{Application.Current.Resources["players.unknown"]}";
        public static BitmapImage GetProfilePhoto(ulong id) => ImageHelper.ConvertPhoto(Users.Where(x => x.Uid == id).Select(x => x.Profile_photo).FirstOrDefault());

        public static PackIconKind GetBadge(Models.UserType Usertype)
        {
            switch (UserType) {
                case Models.UserType.None:
                    return PackIconKind.None;
                case Models.UserType.Tier1:
                    return PackIconKind.HeartOutline;
                case Models.UserType.Tier2:
                    return PackIconKind.HeartMultipleOutline;
                case Models.UserType.Tier3:
                    return PackIconKind.Heart;
                case Models.UserType.Tier4:
                    return PackIconKind.HeartMultiple;
                case Models.UserType.Guider:
                    return PackIconKind.BookAccount;
                case Models.UserType.Moderator:
                    return PackIconKind.Pencil;
                case Models.UserType.Owner:
                    return PackIconKind.Crown;
                default:
                    return PackIconKind.None;
            }
        }

        public static bool IsInternetOn()
        {
            try {
                using (WebClient client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204")) {
                    Library.LastInternetStatus = true;
                    return true;
                }
            }
            catch {
                Library.LastInternetStatus = false;
                return false;
            }
        }

        public static void ClosePrograms()
        {
            foreach (var p in Process.GetProcesses())
                if (p.ProcessName == "FiveM" ||
                    p.ProcessName == "Fivem_ChromeBrowser" ||
                    p.ProcessName == "FiveM_DumpServer" ||
                    p.ProcessName == "FiveM_GTAProcess" ||
                    p.ProcessName == "FiveM_ROSLauncher" ||
                    p.ProcessName == "Fivem_ROSService" ||
                    p.ProcessName == "FiveM_SteamChild" ||
                    p.ProcessName == "ts3client_win64" ||
                    p.ProcessName.ToLower().StartsWith("fivem"))
                    p.Kill();
        }

        private static void SetLocation(string path, string type)
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

        public static bool SetFivemLocation()
        {
            bool retval = false;
            string result = "";

            var dlg = new FolderPicker();
            dlg.InputPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dlg.OkButtonLabel = $"{Application.Current.Resources["dialogs.select-folder"]}";
            dlg.Title = $"{Application.Current.Resources["dialogs.select-folder"]}";

            if (dlg.ShowDialog() == true) {
                result = dlg.ResultPath;
            } else {
                ClosePrograms();
                Environment.Exit(0);
            }

            if (result != "") {
                var f = new FileInfo($@"{result}\FiveM.exe");

                if (f.Exists) {
                    var dataPath = Path.Combine(result, "FiveM.app", "citizen", "common", "data");
                    DirectoryInfo d = new DirectoryInfo(dataPath);

                    if (d.Exists) {
                        SetLocation(result, "fivem");
                        return true;
                    }
                }
                else {
                    MessageBox.Show($"{Application.Current.Resources["mainmenu.nofivematlocation"]}",
                                    $"{Application.Current.Resources["application.title"]}",
                                    MessageBoxButton.OK,
                                    MessageBoxIcon.Error);
                }
            }

            return retval;
        }

        public static void SetGtaVLocation()
        {
            FolderBrowserDialog gtavlocation = new FolderBrowserDialog() {
                Description = $"{Application.Current.Resources["mainmenu.selectgtavlocation"]}"
            };
            var result = gtavlocation.ShowDialog();
            if (result == DialogResult.OK)
                if (File.Exists($@"{gtavlocation.SelectedPath}\GTA5.exe"))
                    SetLocation(gtavlocation.SelectedPath, "gtav");
                else
                    MessageBox.Show($"{Application.Current.Resources["mainmenu.nogtavatlocation"]}",
                                    $"{Application.Current.Resources["application.title"]}",
                                    MessageBoxButton.OK,
                                    MessageBoxIcon.Error);
        }

        public static List<string> GetTags()
        {
            return new List<string> {
                "qb",
                "policejob",
                "highFps",
                "lowPing"
            };
        }
    }
}

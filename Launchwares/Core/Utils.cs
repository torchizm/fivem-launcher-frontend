using Launchwares.Core.Antihack;
using Launchwares.Helpers;
using Launchwares.Properties;
using LaunchwaresCore;
using LaunchwaresCore;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace Launchwares.Core
{
    class Utils
    {
        internal static Models.Server Server { get; set; }
        public static List<Models.User> Users { get; set; }
        public static List<Models.Message> MessageCache = new List<Models.Message>();
        public static List<int> MessageCacheIDs = new List<int>();
        public static IList<string> IllegalPrograms { get; set; }
        public static string Version = "1.0.2.5";
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
            foreach (var proc in Process.GetProcesses())
                if (proc.ProcessName == "FiveM" ||
                    proc.ProcessName == "Fivem_ChromeBrowser" ||
                    proc.ProcessName == "FiveM_DumpServer" ||
                    proc.ProcessName == "FiveM_GTAProcess" ||
                    proc.ProcessName == "FiveM_ROSLauncher" ||
                    proc.ProcessName == "Fivem_ROSService" ||
                    proc.ProcessName == "FiveM_SteamChild" ||
                    proc.ProcessName == "ts3client_win64" ||
                    proc.ProcessName.StartsWith("fivem"))
                    proc.Kill();
        }
    }
}

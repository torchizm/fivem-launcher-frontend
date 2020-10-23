using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace LaunchwaresSubprocess.Helpers
{
    internal static class FivemHelper
    {
        internal static Timer FivemTimer = new Timer() {
            Interval = 5000
        };

        internal static void Start()
        {
            FivemTimer.Elapsed += FivemTimer_Elapsed;
            FivemTimer.Start();
        }

        private static void FivemTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool launcherdetect = false;
            foreach (var Process in Process.GetProcesses()) {
                if (Process.ProcessName.ToLower() == "launchwares") {
                    launcherdetect = true;
                }
            }

            if (!launcherdetect) {
                foreach (var proc in Process.GetProcesses())
                    if (proc.ProcessName == "FiveM" ||
                        proc.ProcessName == "Fivem_ChromeBrowser" ||
                        proc.ProcessName == "FiveM_DumpServer" ||
                        proc.ProcessName == "FiveM_GTAProcess" ||
                        proc.ProcessName == "FiveM_ROSLauncher" ||
                        proc.ProcessName == "Fivem_ROSService" ||
                        proc.ProcessName == "FiveM_SteamChild" ||
                        proc.ProcessName == "ts3client_win64" ||
                        proc.ProcessName.StartsWith("fivem") ||
                        proc.ProcessName.ToLower() == "launchwares") {
                        proc.Kill();
                    }

                Environment.Exit(0);
            }
        }
    }
}

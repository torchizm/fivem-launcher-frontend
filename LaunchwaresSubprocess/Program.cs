using LaunchwaresSubprocess.Helpers;
using System;
using System.Diagnostics;

namespace LaunchwaresSubprocess
{
    class Program
    {
        public static Discord DiscordClient { get; set; }
        public static StartupArgs _Args { get; set; }

        static void Main(string[] args)
        {
            _Args = new StartupArgs(args);

            if (_Args.Length == 0)
            {
                Console.WriteLine("Program düzgün şekilde başlatılamadı.");
                Environment.Exit(0);
            }
            else
            {
                DetectProcess(_Args._Args[0]);
                FivemHelper.Start();
            }

            Console.ReadKey();
        }

        static void DetectProcess(string Key)
        {
            switch (Key)
            {
                case "-rpc_id":
                    SetupDiscord();
                    break;
                case "-kill_program":
                    KillProgram(_Args.GetValue("-kill_program"));
                    break;
                default:
                    break;
            }
        }

        public static void SetupDiscord()
        {
            string id = _Args.GetValue("-rpc_id");
            string rpc_desc = _Args.GetValue("-rpc_description");
            string largeimage_key = _Args.GetValue("-rpc_largeimage_key");
            string largeimage_text = _Args.GetValue("-rpc_largeimage_text");
            string smallimage_key = _Args.GetValue("-rpc_smallimage_key");
            string smallimage_text = _Args.GetValue("-rpc_smallimage_text");
            int max_players = Convert.ToInt32(_Args.GetValue("-max_players").Trim());
            int now_playing = Convert.ToInt32(_Args.GetValue("-now_playing").Trim());
            DiscordClient = new Discord(id, rpc_desc, largeimage_key, largeimage_text, smallimage_key, smallimage_text, Convert.ToInt32(max_players), now_playing);
        }

        public static void KillProgram(string ProgramName)
        {
            foreach (var Proc in Process.GetProcesses())
                if (Proc.ProcessName.ToString() == ProgramName)
                    Proc.Kill();

            Environment.Exit(0);
        }
    }
}

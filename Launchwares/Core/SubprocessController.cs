﻿using System;
using System.Diagnostics;
using System.Text;

namespace Launchwares.Core
{
    public class SubprocessController
    {
        public delegate void OnOutputDataReceived(object sender, DataReceivedEventArgs args);
        public event OnOutputDataReceived OutputDataReceived;

        public SubprocessController(string filename, string arguments = null)
        {
            RunExternalExe(filename, arguments);
        }

        public void RunExternalExe(string filename, string arguments = null)
        {
            var process = new Process();
            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments)) {
                process.StartInfo.Arguments = arguments;
            }

            process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "runas";
            //process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => {
                if (args.Data != null) OutputDataReceived(sender, args);
            };

            try {
                process.Start();
                process.BeginOutputReadLine();
            }
            catch (Exception) {

            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaunchwaresSubprocess.Helpers
{
    public static class RegisteryHelper
    {
        public static void Run()
        {
            RegistryKey scheme = Registry.ClassesRoot.CreateSubKey("vlast");
            scheme.SetValue("", "VLAST Launcher");
            scheme.SetValue("URL Protocol", "");

            var command = scheme.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
            command.SetValue("", $"\"{Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.Length - 21)}/Vlast.exe\" \"%1\"");
        }
    }
}

using Microsoft.Win32;
using System.Reflection;

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

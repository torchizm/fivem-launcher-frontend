using Launchwares.Core;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Launchwares.Core
{
    public static class API
    {
        internal static string Token { get; private set; } = "gVcJNXMuSSkLQTw4Jb6knAW8FMXVzEma";
        internal static Database client = new Database(Token);

        internal static async void GetToken()
        {
            client.OnError += Client_OnError;
            await client.GetToken(Utils.Username,
                            Utils.Uid,
                            Utils.ProfilePhoto,
                            Models.Status.Online);
        }

        private static void Client_OnError(Models.ErrorMessage source, ErrorCode errorCode)
        {
            if (Error.IsCritical(errorCode)) {
                string errorMessage = $"{Application.Current.Resources["error.message"]} ";
                errorMessage += $"{Application.Current.Resources[$"error.{(int)errorCode}"]} \n(Raw: {source.Message}\nCode: {(int)errorCode})";
                MessageBox.Show(errorMessage, $"{Application.Current.Resources["application.title"]}");
                Environment.Exit(-1);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace LaunchwaresCore
{
    public static class Error
    {
        public static Dictionary<string, ErrorCode> Codes = new Dictionary<string, ErrorCode>() {
            { "None", ErrorCode.None },
            { "Ok", ErrorCode.Ok },
            { "Request must be image and max 2mb.", ErrorCode.Ok },
            { "Disliked succesfully.", ErrorCode.Ok },
            { "Unknown", ErrorCode.Unknown },
            { "banned", ErrorCode.Banned },
            { "You are not in whitelist!", ErrorCode.Whitelist },
            { "Launcher in maintenance", ErrorCode.LauncherMaintenance },
            { "Server in maintenance", ErrorCode.ServerMaintenance },
            { "Not enough permissions!", ErrorCode.Forbidden },
            { "Invalid Bearer Token!", ErrorCode.Authorization },
            { "Not a valid Bearer Token!", ErrorCode.Authorization },
            { "No Bearer Token!", ErrorCode.Authorization },
            { "Server Not Found", ErrorCode.ServerNotFound },
            { "This player already exists.", ErrorCode.AlreadyExists }
        };

        public static Dictionary<ErrorCode, bool> Critical = new Dictionary<ErrorCode, bool>() {
            { ErrorCode.Ok, false },
            { ErrorCode.AlreadyExists, false },
            { ErrorCode.Unknown, true },
            { ErrorCode.Authorization, false },
            { ErrorCode.Banned, true },
            { ErrorCode.Forbidden, true },
            { ErrorCode.LauncherMaintenance, true },
            { ErrorCode.None, true },
            { ErrorCode.ServerMaintenance, true },
            { ErrorCode.ServerNotFound, true },
            { ErrorCode.Whitelist, true }
        };

        public static bool IsCritical(ErrorCode code)
        {
            return Critical.Where(x => x.Key == code).Select(x => x.Value).FirstOrDefault();
        }

        public static ErrorCode FromMessage(string message)
        {
            return Codes.TryGetValue(message, out ErrorCode _code) ? _code : 0;
        }

        public static string FromErrorCode(ErrorCode code)
        {
            return (from message in Codes
                    where message.Value == code
                    select message.Key).FirstOrDefault() ?? "Unknown";
        }
    }

    public enum ErrorCode
    {
        None = 0,
        Ok = 30,
        Unknown = 1,
        Whitelist = 2,
        Banned = 3,
        AlreadyExists = 4,
        Authorization = 401,
        Forbidden = 403,
        ServerNotFound = 404,
        LauncherMaintenance = 406,
        ServerMaintenance = 500
    }
}

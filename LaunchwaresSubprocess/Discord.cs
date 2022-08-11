using DiscordRPC;
using LaunchwaresSubprocess.Models;
using Newtonsoft.Json;
using System;

namespace LaunchwaresSubprocess
{
    public class Discord
    {
        public DiscordRpcClient _Client;
        public string _RpcID;
        public string _RpcDescription;
        public string _LargeImageKey;
        public string _LargeImageText;
        public string _SmallImageKey;
        public string _SmallImageText;
        public int _MaxPlayers;
        public int _NowPlaying;

        public Discord(string RpcID, string RpcDescription, string LargeImageKey = null, string LargeImageText = null, string SmallImageKey = null, string SmallImageText = null, int MaxPlayers = 32, int NowPlaying = 0)
        {
            _RpcID = RpcID;
            _RpcDescription = RpcDescription;
            _LargeImageKey = LargeImageKey;
            _LargeImageText = LargeImageText;
            _SmallImageKey = SmallImageKey;
            _SmallImageText = SmallImageText;
            _MaxPlayers = MaxPlayers;
            _NowPlaying = NowPlaying;
            LoadDiscordRPC();
        }

        public Secrets secrets = new Secrets()
        {
            JoinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= ",
            SpectateSecret = "MTIzNDV8MTIzNDV8MTMyNDU0"
        };

        public Timestamps timestamps = new Timestamps()
        {
            Start = DateTime.UtcNow
        };

        public void LoadDiscordRPC()
        {
            _Client = new DiscordRpcClient(_RpcID);

            _Client.Initialize();

            Party party = new Party()
            {
                ID = Secrets.CreateFriendlySecret(new Random()),
                Size = _NowPlaying,
                Max = _MaxPlayers
            };

            _Client.RegisterUriScheme("701972390187499650");
            _Client.SetSubscription(EventType.Join | EventType.Spectate | EventType.JoinRequest);

            _Client.SetPresence(new RichPresence()
            {
                Details = _RpcDescription,
                State = "Launcher",
                Assets = new Assets()
                {
                    LargeImageKey = _LargeImageKey,
                    LargeImageText = _LargeImageText,
                    SmallImageKey = _SmallImageKey,
                    SmallImageText = _SmallImageText
                },
                Secrets = secrets,
                Party = party,
                Timestamps = timestamps
            });

            _Client.OnReady += Client_OnReady;
        }

        public void Client_OnReady(object sender, DiscordRPC.Message.ReadyMessage args) =>
            Console.WriteLine(JsonConvert.SerializeObject(new DiscordOutput(_Client.CurrentUser)));

        public void UpdatePresence()
        {
            _Client.RegisterUriScheme(executable: $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Fivem\FiveM.exe");
            _Client.SetSubscription(EventType.Join | EventType.Spectate);

            Party party = new Party()
            {
                ID = Secrets.CreateFriendlySecret(new Random()),
                Size = _NowPlaying,
                Max = _MaxPlayers
            };

            _Client.SetPresence(new RichPresence()
            {
                Details = _RpcDescription,
                State = "Launcher",
                Assets = new Assets()
                {
                    LargeImageKey = _LargeImageKey,
                    LargeImageText = _LargeImageText,
                    SmallImageKey = _SmallImageKey,
                    SmallImageText = _SmallImageText
                },
                Secrets = secrets,
                Party = party,
                Timestamps = timestamps
            });
        }
    }
}

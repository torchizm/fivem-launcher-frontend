using DiscordRPC;
using Launchwares.Core;
using System;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Launchwares.Discord
{
    public static class Contents
    {
        public static int PlayingCount { get; set; }
    }

    internal static class RichPresence
    {
        internal static DiscordRpcClient client;

        internal static Secrets secrets = new Secrets() {
            JoinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= ",
            SpectateSecret = "MTIzNDV8MTIzNDV8MTMyNDU0"
        };

        internal static Timestamps timestamps = new Timestamps() {
            Start = DateTime.UtcNow
        };

        internal static void LoadDiscordRPC()
        {
            client = new DiscordRpcClient(Properties.Settings.Default.RPCId.ToString());

            /*client.OnPresenceUpdate += (sender, e) =>
            {
                UpdatePresence();
            };*/

            client.Initialize();

            Party party = new Party() {
                ID = Secrets.CreateFriendlySecret(new Random()),
                Size = Contents.PlayingCount,
                Max = Properties.Settings.Default.MaxPlayers
            };

            client.RegisterUriScheme("701972390187499650");
            client.SetSubscription(EventType.Join | EventType.Spectate | EventType.JoinRequest);

            client.SetPresence(new DiscordRPC.RichPresence() {
                Details = Properties.Settings.Default.Description,
                State = "Oynayan:",
                Assets = new Assets() {
                    LargeImageKey = Properties.Settings.Default.RPCLargeImageKey,
                    LargeImageText = Properties.Settings.Default.RPCLargeImageText,
                    SmallImageKey = Properties.Settings.Default.RPCSmallImageKey,
                    SmallImageText = Properties.Settings.Default.RPCSmallImageText
                },
                Secrets = secrets,
                Party = party,
                Timestamps = timestamps
            });

            client.OnReady += Client_OnReady;
        }

        private static void Client_OnReady(object sender, DiscordRPC.Message.ReadyMessage args)
        {
            Utils.Uid = client.CurrentUser.ID;
            Utils.ProfilePhoto = client.CurrentUser.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x256);
            Utils.Username = client.CurrentUser.Username;

            Properties.Settings.Default.PlayerUID = Utils.Uid;
            Properties.Settings.Default.PlayerUsername = Utils.Username;
            Properties.Settings.Default.PlayerProfilephotoPath = Utils.ProfilePhoto;
            Properties.Settings.Default.Save();
        }

        internal static void UpdatePresence()
        {
            client.RegisterUriScheme("701972390187499650");
            client.SetSubscription(EventType.Join | EventType.Spectate);

            Party party = new Party() {
                ID = Secrets.CreateFriendlySecret(new Random()),
                Size = Contents.PlayingCount,
                Max = Convert.ToInt32(Utils.Server.MaxPlayers)
            };

            client.SetPresence(new DiscordRPC.RichPresence() {
                Details = Properties.Settings.Default.Description,
                State = "Oynayan:",
                Assets = new Assets() {
                    LargeImageKey = Properties.Settings.Default.RPCLargeImageKey,
                    LargeImageText = Properties.Settings.Default.RPCLargeImageText,
                    SmallImageKey = Properties.Settings.Default.RPCSmallImageKey,
                    SmallImageText = Properties.Settings.Default.RPCSmallImageText
                },
                Secrets = secrets,
                Party = party,
                Timestamps = timestamps
            });
        }
    }
}
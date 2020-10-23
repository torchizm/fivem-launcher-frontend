using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchwaresSubprocess.Models
{
    public class DiscordOutput
    {
        /// <summary>
        /// Ctx
        /// </summary>
        /// <param name="Client"></param>
        public DiscordOutput(User Client)
        {
            this.Uid = Client.ID;
            this.Username = Client.Username;
            this.ProfilePhoto = Client.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x256);
        }

        public ulong Uid { get; set; }
        public string Username { get; set; }
        public string ProfilePhoto { get; set; }
    }
}

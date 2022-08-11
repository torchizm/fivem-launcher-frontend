using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LaunchwaresCore
{
    public class Models
    {
        [JsonObject]
        public class Token
        {
            [JsonProperty("api_token")]
            public string api_token { get; set; }
            [JsonProperty("slug")]
            public string slug { get; set; }
            [JsonProperty("version")]
            public string version { get; set; }
            [JsonProperty("first")]
            public bool first { get; set; }
        }

        [JsonObject]
        public class User
        {
            [JsonProperty("uid")]
            public ulong Uid { get; set; }
            [JsonProperty("username")]
            public string Username { get; set; }
            [JsonProperty("profile_photo")]
            public string Profile_photo { get; set; }
            [JsonProperty("whitelist")]
            public bool Whitelist { get; set; }
            [JsonProperty("usertype")]
            public int Usertype { get; set; }
            [JsonProperty("ip")]
            public string Ip { get; set; }
            [JsonProperty("status")]
            public int Status { get; set; }
            [JsonProperty("is_banned")]
            public bool IsBanned { get; set; }
            [JsonProperty("created_at")]
            public DateTime JoinedDate { get; set; }
        }

        [JsonObject]
        public class TypedataPass
        {
            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
        }

        [JsonObject]
        public class Server
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("logo_path")]
            public string LogoPath { get; set; }
            [JsonProperty("server_ip")]
            public string ServerIp { get; set; }
            [JsonProperty("server_port")]
            public int? ServerPort { get; set; }
            [JsonProperty("teamspeak_ip")]
            public string TeamspeakIp { get; set; }
            [JsonProperty("teamspeak_port")]
            public int? TeamspeakPort { get; set; }
            [JsonProperty("teamspeak_password")]
            public string TeamspeakPassword { get; set; }
            [JsonProperty("max_players")]
            public int? MaxPlayers { get; set; }
            [JsonProperty("webhook_link")]
            public string WebhookLink { get; set; }
            [JsonProperty("discord_whitelist")]
            public bool DiscordWhitelist { get; set; }
            [JsonProperty("rpc_id")]
            public ulong RpcId { get; set; }
            [JsonProperty("rpc_largeimage_key")]
            public string RpcLargeImageKey { get; set; }
            [JsonProperty("rpc_largeimage_text")]
            public string RpcLargeImageText { get; set; }
            [JsonProperty("rpc_smallimage_key")]
            public string RpcSmallImageKey { get; set; }
            [JsonProperty("rpc_smallimage_text")]
            public string RpcSmallImageText { get; set; }
            [JsonProperty("theme_index")]
            public int ThemeIndex { get; set; }
            [JsonProperty("maintenance")]
            public bool Maintenance { get; set; }
            [JsonProperty("byte_count")]
            public long ByteCount { get; set; }
        }

        [JsonObject]
        public class ServerInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("logo_path")]
            public string LogoPath { get; set; }
        }

        [JsonObject]
        public class FivemPlayer
        {
            [JsonProperty("endpoint")]
            public string endpoint { get; set; }

            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("identifiers")]
            public IList<string> identifiers { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("ping")]
            public int ping { get; set; }
        }

        [JsonObject]
        public class FivemServer
        {
            [JsonProperty("enhancedHostSupport")]
            public bool? EnhancedHostSupport { get; set; }
            //[JsonProperty("icon")]
            //public string Icon { get; set; }
            [JsonProperty("resources")]
            public List<string> Resources { get; set; }
            //[JsonProperty("vars")]
            //public dynamic Vars { get; set; }
            [JsonProperty("version")]
            public int Version { get; set; }
            [JsonProperty("tags")]
            public List<string> Tags { get; set; }
        }

        [JsonObject]
        public class Message
        {
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("sender")]
            public ulong Sender { get; set; }
            [JsonProperty("receiver")]
            public ulong Receiver { get; set; }
            [JsonProperty("is_readed")]
            public bool IsReaded { get; set; }
            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }
            [JsonProperty("id")]
            public int Id { get; set; }
        }

        [JsonObject]
        public class Cheats
        {
            [JsonProperty("CheatNames")]
            public IList<string> CheatNames { get; set; }
        }

        [JsonObject]
        public class Post
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("author")]
            public ulong Author { get; set; }
            [JsonProperty("image_path")]
            public string ImagePath { get; set; }
            [JsonProperty("is_active")]
            public bool IsActive { get; set; }
            [JsonProperty("likes")]
            public List<Like> Likes { get; set; }
            [JsonProperty("created_at")]
            public DateTime Date { get; set; }
        }

        [JsonObject]
        public class Like
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("post_id")]
            public int PostId { get; set; }
            [JsonProperty("player_id")]
            public ulong PlayerId { get; set; }
            [JsonProperty("created_at")]
            public DateTime Date { get; set; }
        }

        [JsonObject]
        public class Update
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("author")]
            public ulong Author { get; set; }
            [JsonProperty("image_path")]
            public string ImagePath { get; set; }
            [JsonProperty("created_at")]
            public DateTime Date { get; set; }
        }

        [JsonObject]
        public class Report
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("author")]
            public ulong Author { get; set; }
            [JsonProperty("target")]
            public ulong Target { get; set; }
        }

        [JsonObject]
        public class Server_vote
        {
            [JsonProperty("player_id")]
            public ulong PlayerId { get; set; }
            [JsonProperty("server_id")]
            public int ServerId { get; set; }
        }

        [JsonObject]
        public class Theme
        {
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("is_private")]
            public bool IsPrivate { get; set; }
            [JsonProperty("user_id")]
            public ulong UserId { get; set; }
            [JsonProperty("theme_path")]
            public string ThemePath { get; set; }
            [JsonProperty("thumbnail_path")]
            public string ThumbnailPath { get; set; }
        }

        [JsonObject]
        public class Badge
        {
            [JsonProperty("server_id")]
            public int ServerId { get; set; }
            [JsonProperty("badge_id")]
            public int BadgeId { get; set; }
        }

        [JsonObject]
        public class ErrorMessage
        {
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("time")]
            public string Time { get; set; }
            [JsonProperty("reason")]
            public string Reason { get; set; }
        }

        [JsonObject]
        public class Blacklist
        {
            [JsonProperty("blacklist")]
            public IList<string> Programs { get; set; }
        }

        [JsonObject]
        public class Hack
        {
            [JsonProperty("cheat_id")]
            public int Client { get; set; }
            [JsonProperty("details")]
            public string Details { get; set; }
            [JsonProperty("player_id")]
            public ulong Player { get; set; }
            [JsonProperty("screenshot_path")]
            public string Image { get; set; }
            [JsonProperty("created_at")]
            public DateTime Date { get; set; }
        }

        [JsonObject]
        public class Character
        {
            [JsonProperty("identifier")]
            public string Identifier { get; set; }
            [JsonProperty("firstname")]
            public string FirstName { get; set; }
            [JsonProperty("lastname")]
            public string LastName { get; set; }
            [JsonProperty("dateofbirth")]
            public string DateOfBirth { get; set; }
            [JsonProperty("sex")]
            public string Sex { get; set; }
            [JsonProperty("job")]
            public string Job { get; set; }
            [JsonProperty("job_grade")]
            public int JobGrade { get; set; }
            [JsonProperty("avatar_url")]
            public string Avatar { get; set; }
        }

        public class FileModel
        {
            [JsonProperty("path")]
            public string Path { get; set; }
        }

        public class Log
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("value")]
            public int Value { get; set; }
            [JsonProperty("created_at")]
            public DateTime Date { get; set; }
        }

        public class GetResponse
        {
            [JsonProperty("response")]
            public bool Response { get; set; }
        }

        [JsonObject]
        public class Hash
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("md5")]
            public string EncryptedString { get; set; }
            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
            [JsonProperty("updated_at")]
            public DateTime UpdatedAT { get; set; }
        }

        public enum Status
        {
            Offline = 0,
            Away = 1,
            Busy = 2,
            Online = 3,
            InGame = 4
        }

        public enum UserType
        {
            None = 0,
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
            Tier4 = 4,
            Guider = 5,
            Moderator = 6,
            Owner = 7
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LaunchwaresCore
{
    public class Database
    {
        public HttpClient Client = new HttpClient();

        public Models.Token Token { get; private set; }
        public string ApiPath { get; private set; } = "http://127.0.0.1:8000/api";
        internal string ApiToken { get; private set; }
        protected string LauncherToken { get; private set; }
        protected string ProtectedLauncherToken { get; } = "pLTj2TY9AjwTpcs44yIgqdwtxaLQ9Fc9";
        internal string DeveloperToken { get; private set; }
        internal bool DebugMode { get; private set; }

        /// <summary>
        /// Token Sync Event
        /// </summary>
        /// <param name="source">Synced Token</param>
        public delegate void OnTokenSyncEventHandler(Models.Token source);
        public event OnTokenSyncEventHandler OnTokenSync;

        /// <summary>
        /// Error Event
        /// </summary>
        /// <param name="source">Error</param>
        /// <param name="errorCode">Error Code</param>
        public delegate void OnErrorEventHandler(Models.ErrorMessage source, ErrorCode errorCode);
        public event OnErrorEventHandler OnError;

        /// <summary>
        /// Client Constructor
        /// </summary>
        /// <param name="LauncherToken">Launcher Token from www.launchwares.com</param>
        public Database(string LauncherToken) => this.LauncherToken = LauncherToken;

        /// <summary>
        /// Developer Constructor
        /// </summary>
        /// <param name="LauncherToken">Launcher token from www.launchwares.com</param>
        /// <param name="DebugMode">Debug Mode true or false</param>
        public Database(string LauncherToken, bool DebugMode)
        {
            this.LauncherToken = LauncherToken;
            this.DeveloperToken = LauncherToken;
            this.DebugMode = DebugMode;
        }

        /// <summary>
        /// Returns Token
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="id">Id</param>
        /// <param name="profilePhoto">Profile Photo URL</param>
        /// <param name="ip">IP Address</param>
        /// <param name="status">Status</param>
        /// <returns>Models.Token</returns>
        public async Task<Models.Token> GetToken(string username, ulong id, string profilePhoto, Models.Status status)
        {
            if (this.ProtectedLauncherToken == "" || username == null || id == 0 || profilePhoto == null) return null;
            var token = await Post<Models.Token>($"launcher_login?launcherToken={ProtectedLauncherToken}&uid={id}&uname={username}&pp={profilePhoto}&status={(int)status}");

            if (token.api_token == null || token.api_token == "") return new Models.Token()
            {
                api_token = "null",
                slug = "null",
                version = "null",
                first = false
            };

            if (DebugMode && (token.api_token == null || token.api_token == "")) Log("Token can not synced", ErrorCode.Authorization);

            this.Token = token;
            OnTokenSync(token);
            return token;
        }

        /// <summary>
        /// Sends GET process to API and returns object you gave
        /// </summary>
        /// <typeparam name="T">Object as you wish</typeparam>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> Get<T>(string path, bool useToken = false, bool checkError = false)
        {
            if (useToken) path += $"&launcher_token={this.ProtectedLauncherToken}";
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            if (checkError)
            {
                try
                {
                    var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                    if (HttpError.Message != null && HttpError.Message != "Ok") OnError(HttpError, Error.FromMessage(HttpError.Message));
                }
                catch { }
            }

            if (DebugMode) Log($"Method: GET\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Sends GET process to API
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task Get(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null && HttpError.Message != "Ok") OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: GET\nStatus: {response.StatusCode}\nMessage: {message}");
        }

        /// <summary>
        /// Sends GET process to API and returns object you gave
        /// </summary>
        /// <typeparam name="T">Object as you wish</typeparam>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> CustomGet<T>(string path, bool useToken = false, bool checkError = false)
        {
            if (useToken) path += $"&launcher_token={this.ProtectedLauncherToken}";
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            if (checkError)
            {
                try
                {
                    var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                    if (HttpError.Message != null && HttpError.Message != "Ok") OnError(HttpError, Error.FromMessage(HttpError.Message));
                }
                catch { }
            }

            if (DebugMode) Log($"Method: GET\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Sends POST process to API
        /// </summary>
        /// <typeparam name="T">Object T</typeparam>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> Post<T>(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: POST\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Sends POST process to API
        /// </summary>
        /// <typeparam name="T">Object T</typeparam>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> CustomPost<T>(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{path}&token={this.ProtectedLauncherToken}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            //try
            //{
            //    var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
            //    if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            //}
            //catch { }

            if (DebugMode) Log($"Method: POST\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Sends POST process to API
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task Post(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: POST\nStatus: {response.StatusCode}\nMessage: {message}");
        }

        /// <summary>
        /// Sends DELETE process to API
        /// </summary>
        /// <typeparam name="T">Object T</typeparam>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> Delete<T>(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: DELETE\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Sends DELETE process to API
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task Delete(string path)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{ApiPath}/{path}");

            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: DELETE\nStatus: {response.StatusCode}\nMessage: {message}");
        }

        /// <summary>
        /// Sends HttpMethod to API
        /// </summary>
        /// <typeparam name="T">Object T</typeparam>
        /// <param name="method">Http Method</param>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task<T> Execute<T>(HttpMethod method, string path)
        {
            var request = new HttpRequestMessage(method, $"{ApiPath}/{path}");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: {method.Method}\nStatus: {response.StatusCode}\nMessage: {message}");

            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Uploads image to image service
        /// </summary>
        /// <param name="path">Local Image Path</param>
        /// <returns></returns>
        public async Task<string> UploadImage(string path)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/photo/upload");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(path)), "image", Path.GetFileName(path));
            request.Content = multipartContent;

            var response = await httpClient.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            var file = JsonConvert.DeserializeObject<Models.FileModel>(message);
            return $"{ApiPath.Substring(0, ApiPath.Length - 3)}storage/{file.Path}";
        }

        /// <summary>
        /// Uploads file to the server
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(string path)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/cdn/upload");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(path)), "file", Path.GetFileName(path));
            request.Content = multipartContent;

            var response = await httpClient.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            var file = JsonConvert.DeserializeObject<Models.FileModel>(message);
            return $"{ApiPath.Substring(0, ApiPath.Length - 3)}storage/{file.Path}";
        }

        public async Task<string> UploadImageWithoutPath(byte[] bytes)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/photo/upload");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new ByteArrayContent(bytes), "image", "xfile.png");
            request.Content = multipartContent;

            var response = await httpClient.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            var file = JsonConvert.DeserializeObject<Models.FileModel>(message);
            return $"{ApiPath.Substring(0, ApiPath.Length - 3)}storage/{file.Path}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task<string> UploadFileWithoutPath(byte[] bytes)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiPath}/cdn/upload");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new ByteArrayContent(bytes), "file", "xfile.png");
            request.Content = multipartContent;

            var response = await httpClient.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            var file = JsonConvert.DeserializeObject<Models.FileModel>(message);
            return $"{ApiPath.Substring(0, ApiPath.Length - 3)}storage/{file.Path}";
        }

        /// <summary>
        /// Sends HttpMethod to API
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="path">Path</param>
        /// <returns>T</returns>
        public async Task Execute(HttpMethod method, string path)
        {
            var request = new HttpRequestMessage(method, $"{ApiPath}/{path}");
            if (Token != null && Token.api_token != "") request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Token.api_token}");

            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var HttpError = JsonConvert.DeserializeObject<Models.ErrorMessage>(message);
                if (HttpError.Message != null) OnError(HttpError, Error.FromMessage(HttpError.Message));
            }
            catch { }

            if (DebugMode) Log($"Method: {method.Method}\nStatus: {response.StatusCode}\nMessage: {message}");
        }

        /// <summary>
        /// Debug Mode Log Void
        /// </summary>
        /// <param name="Message">Message (Default Null)</param>
        /// <param name="Code">ErrorCode (Default None (0))</param>
        private void Log(string Message = null, ErrorCode Code = ErrorCode.None)
        {
            var msg = "";

            if (Message != null || Message != "") msg += Message;
            if (Code != ErrorCode.None) msg += $"\nError: {Error.FromErrorCode(Code)} ({(int)Code})";

            if (msg != null && msg != "") Console.WriteLine(msg);
            return;
        }

        private void ErrorLog(string Message = null, ErrorCode Code = ErrorCode.None)
        {
            var msg = "";

            if (Message != null || Message != "") msg += Message;
            if (Code != ErrorCode.None) msg += $"\nError: {Error.FromErrorCode(Code)} ({(int)Code})";

            if (msg != null && msg != "")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ResetColor();
            }

            return;
        }
    }
}
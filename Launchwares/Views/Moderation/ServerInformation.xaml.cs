using Launchwares.Core;
using Launchwares.CustomElements;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Launchwares.Views.Moderation
{
    /// <summary>
    /// ServerInformation.xaml etkileşim mantığı
    /// </summary>
    public partial class ServerInformation : Page
    {
        HttpClient Client = new HttpClient();

        public ServerInformation()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetInformation();
        }

        public void GetInformation()
        {
            //try {
            //    var Response = await API.client.CustomGet<Models.FivemServer>($"http://{Utils.Server.ServerIp}:{Utils.Server.ServerPort}/info.json");
            //}catch(HttpRequestException exception) {
            //    MessageBox.Show("Sunucu ile bağlantı kurulamadı.\nHata detayı:\n" + exception.ToString());
            //}

            for (int i = 0; i < 15; i++) {
                Utils.GetTags().ForEach(x => {
                    this.TagContainer.Children.Add(new MiniTagBox(x));
                });
            }
        }
    }
}
using Launchwares.Core;
using LaunchwaresCore;
using System.Windows;
using System.Windows.Controls;

namespace Launchwares.Views
{
    /// <summary>
    /// Report.xaml etkileşim mantığı
    /// </summary>
    public partial class Report : Page
    {
        ulong user = 0;
        public Report(ulong id)
        {
            InitializeComponent();
            user = id;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (Contents.Text == Application.Current.Resources["report.content"].ToString()
             || Contents.Text == "") return;

            await API.client.Post<Models.Report>($"report/{API.client.Token.slug}?content={Contents.Text}&author={Utils.Uid}&player={user}&cat={CategorySelector.SelectedIndex + 1}");

            MessageBox.Show($"{Application.Current.Resources["report.ok"]}", $"{Application.Current.Resources["application.title"]}", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

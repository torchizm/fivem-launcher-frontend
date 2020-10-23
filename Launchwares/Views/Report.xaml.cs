using Launchwares.Core;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

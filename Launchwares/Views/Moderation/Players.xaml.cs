using Launchwares.Core;
using Launchwares.CustomElements;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Launchwares.Views.Moderation
{
    /// <summary>
    /// Players.xaml etkileşim mantığı
    /// </summary>
    public partial class Players : Page
    {
        public Players()
        {
            InitializeComponent();
            GetPlayers();
        }

        public async void GetPlayers()
        {
            Utils.Users = await API.client.Get<List<Models.User>>($"players/{API.client.Token.slug}");

            foreach (var user in Utils.Users)
                PlayerContainer.Children.Add(new PlayerBox(user));
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlayerContainer.Children.Clear();

            if (SearchBox.Text == "") {
                foreach (var user in Utils.Users)
                    PlayerContainer.Children.Add(new PlayerBox(user));

                return;
            }

            if (MainWindow.IsNumber(SearchBox.Text))
                foreach (var user in Utils.Users.Where(x => Regex.IsMatch(x.Uid.ToString(), SearchBox.Text)))
                    PlayerContainer.Children.Add(new PlayerBox(user));
            else
                foreach (var user in Utils.Users.Where(x => Regex.IsMatch(x.Username, SearchBox.Text)))
                    PlayerContainer.Children.Add(new PlayerBox(user));
        }
    }
}

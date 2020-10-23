using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
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

namespace Launchwares.CustomElements
{
    /// <summary>
    /// PlayerBox.xaml etkileşim mantığı
    /// </summary>
    public partial class PlayerBox : UserControl
    {
        private bool PlayerLoaded = false;

        public PlayerBox(Models.User Player)
        {
            InitializeComponent();
            this.Player = Player;

            PlayerProfilePhoto.ImageSource = ImageHelper.ConvertPhoto(Player.Profile_photo);
            PlayerUsername.Text = Player.Username;
            HasWhitelist.IsChecked = Player.Whitelist;
            PermissionBox.SelectedIndex = Player.Usertype;
            PlayerLoaded = true;
        }

        public Models.User Player { get; set; }

        private void HasWhitelist_Click(object sender, RoutedEventArgs e)
        {
            if (!PlayerLoaded) return;
            SavePlayer();
        }

        private void PermissionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!PlayerLoaded) return;
            SavePlayer();
        }

        public async void SavePlayer()
        {
            if (!PlayerLoaded) return;
            await API.client.Post($"player/{API.client.Token.slug}?player={Player.Uid}&whitelist={GetBoolValue(HasWhitelist.IsChecked)}&usertype={PermissionBox.SelectedIndex}");
        }

        private int GetBoolValue(bool? val) => (val == true) ? 1 : 0;
    }
}

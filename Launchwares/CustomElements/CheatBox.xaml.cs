using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launchwares.CustomElements
{
    /// <summary>
    /// CheatBox.xaml etkileşim mantığı
    /// </summary>
    public partial class CheatBox : UserControl
    {
        public CheatBox(Models.Hack Cheat)
        {
            InitializeComponent();
            Models.User Player = new Models.User();
            try {
                Player = Utils.Users.Where(x => x.Uid == Cheat.Player).FirstOrDefault();
                SuspectImage.ImageSource = ImageHelper.ConvertPhoto(Player.Profile_photo);
                SuspectUsername.Text = Player.Username;
            } catch (Exception) {
                SuspectImage.ImageSource = ImageHelper.ConvertPhoto("http://api.vlastcommunity.net/img/vlast.png");
                SuspectUsername.Text = $"{Application.Current.Resources["players.unknown"]}";
            }

            if (Cheat.Client == 0)
                CheatName.Text = "Dosya ismi bilinmiyor";
            else
                CheatName.Text = Utils.IllegalPrograms[Cheat.Client - 1];

            if (!string.IsNullOrWhiteSpace(Cheat.Details) && !string.IsNullOrEmpty(Cheat.Details)) {
                this.UrlSource = Cheat.Image;
                DetailsText.Visibility = Visibility.Visible;
                DetailsText.Text = Cheat.Details;
            }
            
            DateText.Text = Cheat.Date.ToString();

            if (Cheat.Image != null && Cheat.Image != "" && (
                    Cheat.Image.EndsWith("jpeg") || Cheat.Image.EndsWith("jpg") || Cheat.Image.EndsWith("png")
                )) {
                this.UrlSource = Cheat.Image;
                PhotoSource.ImageSource = ImageHelper.ConvertPhoto(Cheat.Image);
            } else {
                PhotoContainer.Visibility = Visibility.Collapsed;
            }
        }

        public string UrlSource { get; set; }

        private void PhotoContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            Process.Start(this.UrlSource);
        }

        private void DetailsText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            Process.Start(this.UrlSource);
        }
    }
}

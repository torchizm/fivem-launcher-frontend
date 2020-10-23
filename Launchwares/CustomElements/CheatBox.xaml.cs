using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            }
            catch (Exception) {
                SuspectImage.ImageSource = ImageHelper.ConvertPhoto("https://launchwares.com/img/launchwares.png");
                SuspectUsername.Text = $"{Application.Current.Resources["players.unknown"]}";
            }

            CheatName.Text = Utils.IllegalPrograms[Cheat.Client - 1];
            DateText.Text = Cheat.Date.ToString();

            if (Cheat.Image != null && Cheat.Image != "") {
                this.ImageSource = Cheat.Image;
                PhotoSource.ImageSource = ImageHelper.ConvertPhoto(Cheat.Image);
            }
            else {
                PhotoContainer.Visibility = Visibility.Collapsed;
            }
        }

        public string ImageSource { get; set; }

        private void PhotoContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            Process.Start(this.ImageSource);
        }
    }
}

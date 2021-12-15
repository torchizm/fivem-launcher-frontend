using Launchwares.Core;
using Launchwares.CustomElements;
using LaunchwaresCore;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Launchwares.Views
{
    /// <summary>
    /// Updates.xaml etkileşim mantığı
    /// </summary>
    public partial class Updates : Page
    {
        public Updates()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PublishContainer.Visibility = (Utils.UserType >= Models.UserType.Guider) ? Visibility.Visible : Visibility.Collapsed;

            var updates = await API.client.Get<IList<Models.Update>>($"updates/{API.client.Token.slug}");

            if (updates == null)
                return;

            foreach (var update in updates)
                this.UpdateContainer.Children.Add(new BigUpdateBox(update));
        }

        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentText.Text.Trim() == "") return;
            if (SelectedImagePath.Text != "") {
                ShareTransitioner.SelectedIndex = 1;
                ShareButton.IsEnabled = false;
                var image = await API.client.UploadImage(SelectedImagePath.Text);
                await API.client.Post($"updates/{API.client.Token.slug}?title={TitleText.Text}&content={ContentText.Text}&uid={Utils.Uid}&image_path={image}");
                TitleText.Text = "";
                ContentText.Text = "";
                SelectedImagePath.Text = "";
                ShareButton.IsEnabled = true;
                ShareTransitioner.SelectedIndex = 0;
            }
            else {
                ShareTransitioner.SelectedIndex = 1;
                ShareButton.IsEnabled = false;
                await API.client.Post($"updates/{API.client.Token.slug}?title={TitleText.Text}&content={ContentText.Text}&uid={Utils.Uid}");
                TitleText.Text = "";
                ContentText.Text = "";
                SelectedImagePath.Text = "";
                ShareButton.IsEnabled = true;
                ShareTransitioner.SelectedIndex = 0;
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            fd.Title = "Select Photos";

            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK) {
                SelectedImagePath.Text = fd.FileName;
            }
        }
    }
}

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
    /// Posts.xaml etkileşim mantığı
    /// </summary>
    public partial class Posts : Page
    {
        public Posts()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var PostsList = await API.client.Get<List<Models.Post>>($"posts/{API.client.Token.slug}");

            if (PostsList == null)
                return;

            int i = 0;
            foreach (var post in PostsList) {
                if (i % 2 == 0)
                    PostContainerLeft.Children.Add(new PostBox(post));
                else
                    PostContainerRight.Children.Add(new PostBox(post));
                i++;
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            fd.Title = "Select Photos";

            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK) {
                SelectedImagePath.Text = fd.FileName;
            }
        }

        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentText.Text.Trim() == "") return;
            if (SelectedImagePath.Text != "") {
                ShareTransitioner.SelectedIndex = 1;
                ShareButton.IsEnabled = false;
                var image = await API.client.UploadImage(SelectedImagePath.Text);
                await API.client.Post($"post/{API.client.Token.slug}?content={ContentText.Text}&uid={Utils.Uid}&image_path={image}");
                ContentText.Text = "";
                SelectedImagePath.Text = "";
                ShareButton.IsEnabled = true;
                ShareTransitioner.SelectedIndex = 0;
            }
            else {
                ShareTransitioner.SelectedIndex = 1;
                ShareButton.IsEnabled = false;
                await API.client.Post($"post/{API.client.Token.slug}?content={ContentText.Text}&uid={Utils.Uid}");
                ContentText.Text = "";
                SelectedImagePath.Text = "";
                ShareButton.IsEnabled = true;
                ShareTransitioner.SelectedIndex = 0;
            }
        }
    }
}

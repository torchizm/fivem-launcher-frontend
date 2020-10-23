using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using MaterialDesignThemes.Wpf;
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
    /// MiniPostBox.xaml etkileşim mantığı
    /// </summary>
    public partial class PostBox : UserControl
    {
        public PostBox()
        {
            InitializeComponent();
        }

        public PostBox(Models.Post post)
        {
            InitializeComponent();

            ActionsButton.Visibility = (Utils.UserType >= Models.UserType.Guider) ? Visibility.Visible : Visibility.Hidden;
            ActiveButton.Visibility = (post.IsActive) ? Visibility.Collapsed : Visibility.Visible;
            NotActiveNotification.Visibility = (post.IsActive) ? Visibility.Collapsed : Visibility.Visible;
            this.PostId = post.Id;
            this.Publisher = post.Author;
            this.AuthorImage.ImageSource = Utils.GetProfilePhoto(post.Author);
            this.Text = post.Content;
            this.Date = DateHelper.GetDiffForHumans(post.Date);

            if (string.IsNullOrWhiteSpace(post.ImagePath)) {
                this.PhotoContainer.Visibility = Visibility.Collapsed;
            }
            else {
                this.ImagePath = ImageHelper.ConvertPhoto(post.ImagePath);
            }

            LikeCount.Text = post.Likes.Count.ToString();
            LikeIcon.Foreground = (post.Likes.Where(x => x.PlayerId == Utils.Uid).FirstOrDefault() == null) ? Brushes.Black : Brushes.Red;
            LikeIcon.Kind = (post.Likes.Where(x => x.PlayerId == Utils.Uid).FirstOrDefault() == null) ? PackIconKind.HeartOutline : PackIconKind.Heart;
        }

        public int PostId { get; set; }

        public string Text
        {
            get
            {
                return DescriptionText.Text;
            }
            set
            {
                if (DescriptionText.Text != value)
                    DescriptionText.Text = value;
            }
        }

        public ulong Publisher
        {
            get
            {
                return ulong.Parse(PublisherText.Text);
            }
            set
            {
                PublisherText.Text = Utils.GetUsername(value);
            }
        }

        public ImageSource PublisherImage
        {
            get
            {
                return AuthorImage.ImageSource;
            }
            set
            {
                AuthorImage.ImageSource = value;
            }
        }

        public string Date
        {
            get
            {
                return DateText.Text;
            }
            set
            {
                if (DateText.Text != value)
                    DateText.Text = value;
            }
        }

        public ImageSource ImagePath
        {
            get
            {
                return PhotoSource.ImageSource;
            }
            set
            {
                if (PhotoSource.ImageSource != value)
                    PhotoSource.ImageSource = value;
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            LikeCount.Text = (LikeIcon.Kind == PackIconKind.HeartOutline) ? (int.Parse(LikeCount.Text) + 1).ToString() : (int.Parse(LikeCount.Text) - 1).ToString();
            LikeIcon.Foreground = (LikeIcon.Foreground == Brushes.Black) ? Brushes.Red : Brushes.Black;
            LikeIcon.Kind = (LikeIcon.Kind == PackIconKind.HeartOutline) ? PackIconKind.Heart : PackIconKind.HeartOutline;
            await API.client.Post($"post/like/{API.client.Token.slug}/{this.PostId}");
        }

        private async void DeleteButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            await API.client.Post($"post/delete/{API.client.Token.slug}/{this.PostId}");
            this.TransitionContainer.SelectedIndex = 1;
        }

        private async void ActiveButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            ActiveButton.Visibility = Visibility.Collapsed;
            NotActiveNotification.Visibility = Visibility.Collapsed;
            await API.client.Post($"post/active/{API.client.Token.slug}/{this.PostId}");
        }
    }
}

using Launchwares.AnimationHelpers;
using Launchwares.Core;
using Launchwares.CustomElements;
using Launchwares.Helpers;
using LaunchwaresCore;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launchwares.CustomElements
{
    /// <summary>
    /// BigPostBox.xaml etkileşim mantığı
    /// </summary>
    public partial class BigUpdateBox : UserControl
    {
        public BigUpdateBox()
        {
            InitializeComponent();
            DeleteButton.Visibility = (Utils.UserType >= Models.UserType.Guider) ? Visibility.Visible : Visibility.Hidden;
        }

        public BigUpdateBox(Models.Update update)
        {
            InitializeComponent();

            DeleteButton.Visibility = (Utils.UserType >= Models.UserType.Guider) ? Visibility.Visible : Visibility.Hidden;
            this.UpdateId = update.Id;
            this.Title = update.Title;
            this.Publisher = Utils.GetUsername(update.Author);
            this.Description = update.Content;
            this.PhotoSource = ImageHelper.ConvertPhoto(update.ImagePath);
            this.Date = DateHelper.GetDiffForHumans(update.Date);
        }

        public int UpdateId { get; set; }

        public string Title
        {
            get
            {
                return TitleText.Text;
            }
            set
            {
                if (TitleText.Text != value)
                    TitleText.Text = value;
            }
        }

        public string Description
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

        public string Publisher
        {
            get
            {
                return PublisherText.Text;
            }
            set
            {
                if (PublisherText.Text != value)
                    PublisherText.Text = value;
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

        public ImageSource PhotoSource
        {
            get
            {
                return PhotoSourceText.ImageSource;
            }
            set
            {
                if (PhotoSourceText.ImageSource != value)
                    PhotoSourceText.ImageSource = value;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionContainer.SelectedIndex = 1;
        }

        private async void YesButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionContainer.SelectedIndex = 2;
            await API.client.Post($"update/delete/{API.client.Token.slug}/{this.UpdateId}");
            this.Visibility = Visibility.Collapsed;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionContainer.SelectedIndex = 0;
        }
    }
}

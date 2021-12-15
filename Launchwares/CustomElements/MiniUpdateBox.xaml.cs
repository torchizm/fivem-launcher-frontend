using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Launchwares.CustomElements
{
    /// <summary>
    /// PostBox.xaml etkileşim mantığı
    /// </summary>
    public partial class MiniUpdateBox : UserControl
    {
        public MiniUpdateBox()
        {
            InitializeComponent();
        }

        public MiniUpdateBox(Models.Update update)
        {
            InitializeComponent();

            Title = update.Title;
            try {
                Publisher = Utils.GetUsername(update.Author);
                PhotoSource = ImageHelper.ConvertPhoto(update.ImagePath);
            }
            catch (Exception) {
                PhotoSource = ImageHelper.ConvertPhoto("http://api.vlastcommunity.net/img/vlast.png");
                Publisher = $"{Application.Current.Resources["players.unknown"]}";
            }
            Date = DateHelper.GetDiffForHumans(update.Date);
        }


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
    }
}

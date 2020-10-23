using Launchwares.Core;
using Launchwares.Helpers;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
                PhotoSource = ImageHelper.ConvertPhoto("https://launchwares.com/img/launchwares.png");
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

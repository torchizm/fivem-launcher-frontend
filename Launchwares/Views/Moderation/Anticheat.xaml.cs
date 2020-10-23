using Launchwares.Core;
using Launchwares.CustomElements;
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

namespace Launchwares.Views.Moderation
{
    /// <summary>
    /// Anticheat.xaml etkileşim mantığı
    /// </summary>
    public partial class Anticheat : Page
    {
        public Anticheat()
        {
            InitializeComponent();
            GetDetections();
        }

        public async void GetDetections()
        {
            var hackList = await API.client.Get<IList<Models.Hack>>($"cheats/{API.client.Token.slug}");

            if (hackList == null) return;

            foreach (var hack in hackList)
                DetectionContainer.Children.Add(new CheatBox(hack));
        }
    }
}

using Launchwares.Core;
using Launchwares.CustomElements;
using LaunchwaresCore;
using System.Collections.Generic;
using System.Windows.Controls;

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

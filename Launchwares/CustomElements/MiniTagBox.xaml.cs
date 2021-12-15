using System.Windows.Controls;

namespace Launchwares.CustomElements
{
    /// <summary>
    /// MiniTagBox.xaml etkileşim mantığı
    /// </summary>
    public partial class MiniTagBox : UserControl
    {
        public MiniTagBox()
        {
            InitializeComponent();
        }

        public MiniTagBox(string TagLabel)
        {
            InitializeComponent();
            this.TagLabel.Text = TagLabel;
        }
    }
}

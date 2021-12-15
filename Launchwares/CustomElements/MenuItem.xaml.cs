using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Launchwares.CustomElements
{
    /// <summary>
    /// MenuItem.xaml etkileşim mantığı
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public MenuItem()
        {
            InitializeComponent();
        }

        public static DependencyProperty CurrentTextProperty = DependencyProperty.Register("MenuItemText", typeof(string), typeof(MenuItem), new PropertyMetadata(new PropertyChangedCallback(OnTextChanged)));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuItem viewer = d as MenuItem;
            viewer.MenuItemText = ((string)e.NewValue);
        }

        public string MenuItemText
        {
            get
            {
                return ItemText.Text;
            }
            set
            {
                if (ItemText.Text != value)
                    ItemText.Text = value;
            }
        }

        public PackIconKind Icon
        {
            get
            {
                return ItemIcon.Kind;
            }
            set
            {
                if (ItemIcon.Kind != value)
                    ItemIcon.Kind = value;
            }
        }
    }
}

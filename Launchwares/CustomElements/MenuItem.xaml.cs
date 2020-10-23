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

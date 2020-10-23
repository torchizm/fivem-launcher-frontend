using Launchwares.Resources.Design;
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
    /// LanguageOption.xaml etkileşim mantığı
    /// </summary>
    public partial class LanguageOption : UserControl
    {
        public LanguageOption()
        {
            InitializeComponent();
        }

        public static DependencyProperty CurrentTextProperty = DependencyProperty.Register("LanguageOptionText", typeof(string), typeof(LanguageOption), new PropertyMetadata(new PropertyChangedCallback(OnTextChanged)));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LanguageOption viewer = d as LanguageOption;
            viewer.LanguageOptionText = ((string)e.NewValue);
        }

        public string LanguageKey { get; set; }

        public string LanguageOptionText
        {
            get
            {
                return LanguageText.Text;
            }
            set
            {
                if (LanguageText.Text != value)
                    LanguageText.Text = value;
            }
        }

        public bool? IsChecked
        {
            get
            {
                return Toggle.IsChecked;
            }
            set
            {
                if (Toggle.IsChecked != value) {
                    OptionContainer.Style = (Toggle.IsChecked == true) ? ThemeHelper.LanguageOptionStyle : ThemeHelper.LanguageOptionSelectedStyle;
                    Toggle.IsChecked = value;
                }
            }
        }

        public string GlobalLanguage
        {
            get
            {
                return GlobalLanguageText.Text;
            }
            set
            {
                if (GlobalLanguageText.Text != value)
                    GlobalLanguageText.Text = value;
            }
        }

        public ImageSource FlagPath
        {
            get
            {
                return Flag.Source;
            }
            set
            {
                if (Flag.Source != value)
                    Flag.Source = value;
            }
        }

        public string Value
        {
            get
            {
                return Value;
            }
            set
            {
                if (Value != value)
                    Value = value;
            }
        }
    }
}
